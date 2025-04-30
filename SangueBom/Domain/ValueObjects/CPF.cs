using System.Text.RegularExpressions;

namespace DoadoresDeSangue.Domain.ValueObjects
{
    public class CPF
    {
        public string Valor { get; }

        public CPF(string valor)
        {
            if (!EhValido(valor))
                throw new ArgumentException("CPF inválido.");

            Valor = SomenteNumeros(valor);
        }

        private string SomenteNumeros(string input)
        {
            return Regex.Replace(input, @"[^\d]", "");
        }

        private bool EhValido(string cpf)
        {
            cpf = SomenteNumeros(cpf);

            if (cpf.Length != 11) return false;

            if (cpf.All(c => c == cpf[0])) return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public override bool Equals(object? obj)
        {
            return obj is CPF other && Valor == other.Valor;
        }

        public override int GetHashCode()
        {
            return Valor.GetHashCode();
        }

        public override string ToString() => Convert.ToUInt64(Valor).ToString(@"000\.000\.000\-00");
    }
}
