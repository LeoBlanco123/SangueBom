using SangueBom.Domain.Entities;
using SangueBom.Domain.Enums;

namespace SangueBom.Domain.Services
{
    public class ValidadorDeDoacaoService
    {
        private const int IntervaloHomem = 60;
        private const int IntervaloMulher = 90;

        public bool PodeRealizarDoacao(Doador doador, DateTime? dataUltimaDoacao, DateTime dataAtual)
        {
            if (dataUltimaDoacao is null) return true;

            double dias = (dataAtual - dataUltimaDoacao.Value).TotalDays;

            return doador.Genero switch
            {
                Genero.Masculino => dias >= IntervaloHomem,
                Genero.Feminino => dias >= IntervaloMulher,
                _ => false
            };
        }
    }
}
