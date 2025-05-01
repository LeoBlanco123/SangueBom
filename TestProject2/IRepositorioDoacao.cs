using SangueBom.Domain.Entities;

namespace SangueBom.Domain.Repositories
{
    public interface IRepositorioDoacao
    {
        Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId);
        Task RegistrarAsync(Doacao doacao);
    }
}
