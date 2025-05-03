using SangueBom.Domain.Entities;

namespace SangueBom.Domain.Interfaces
{
    public interface ICadastroDoadorRepositorio
    {
        Task<List<Doador>> ListarAsync();
        Task<Doador> ObterPorCpfAsync(string cpf);
        Task CadastrarAsync(Doador doador);
        Task AdicionarAsync(Doador doador);
        Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId);
        Task<Doador> ObterPorIdAsync(Guid doadorId);
    }
}
