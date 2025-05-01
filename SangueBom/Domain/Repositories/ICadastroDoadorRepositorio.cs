using SangueBom.Domain.Entities;

namespace SangueBom.Domain.Interfaces
{
    public interface ICadastroDoadorRepositorio
    {
        Task<List<Doador>> ListarAsync();
        Task<Doador> ObterPorCpfAsync(string cpf);
        Task CadastrarAsync(Doador doador);
        Task<Doador> ObterPorIdAsync(Guid doadorId);
    }
}
