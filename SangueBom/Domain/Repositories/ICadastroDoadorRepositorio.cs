using SangueBom.Domain.Entities;

namespace SangueBom.Domain.Interfaces
{
    public interface ICadastroDoadorRepositorio
    {
        Task<Doador?> ObterPorCpfAsync(string cpf);
        Task CadastrarAsync(Doador doador);
    }
}
