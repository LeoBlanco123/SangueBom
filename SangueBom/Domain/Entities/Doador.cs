using DoadoresDeSangue.Domain.ValueObjects;
using SangueBom.Domain.Enums;
using SangueBom.Domain.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

namespace SangueBom.Domain.Entities
{
    public class Doador
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public CPF Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public Genero Genero { get; private set; }
        public TipoSanguineo TipoSanguineo { get; private set; }
        public Endereco Endereco { get; private set; }
        public string Telefone { get; private set; }


        public int Idade => CalcularIdade(DataNascimento);

        public object CPF { get; internal set; }

        public Doador(string nome, CPF cpf, DateTime dataNascimento, Genero genero, TipoSanguineo tipoSanguineo, Endereco endereco, string telefone)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.");

            if (CalcularIdade(dataNascimento) < 18)
                throw new ArgumentException("Doador deve ter 18 anos ou mais.");

            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Genero = genero;
            TipoSanguineo = tipoSanguineo;
            Endereco = endereco; // Certifique-se de que o objeto Endereco é válido
            Telefone = telefone;
        }


        private int CalcularIdade(DateTime nascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - nascimento.Year;

            if (nascimento.Date > hoje.AddYears(-idade)) idade--;

            return idade;
        }

        private void CadastrarDoador(string rua, string numero, string bairro, string cidade, string estado)
        {
            Endereco = new Endereco(rua, numero, bairro, cidade, estado);

            Console.WriteLine($"Doador: {Nome}, CPF: {Cpf}, Data de Nascimento: {DataNascimento:dd/MM/yyyy}, Tipo Sanguíneo: {TipoSanguineo}, Gênero: {Genero}, Idade: {Idade}, Telefone: {Telefone}");
            Console.WriteLine($"Endereço: Rua {Endereco.Rua}, Número {Endereco.Numero}, Bairro {Endereco.Bairro}, Cidade {Endereco.Cidade}, Estado {Endereco.Estado}");
        }
    }
}
