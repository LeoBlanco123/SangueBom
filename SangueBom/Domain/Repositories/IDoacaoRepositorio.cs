using SangueBom.Domain.Entities;

namespace SangueBom.Domain.Repositories
{
    public interface IDoacaoRepositorio
    {
        Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId);
        Task<Doacao?> ObterUltimaPorDoadorIdAsync(Guid doadorId);
        Task RegistrarAsync(Doacao doacao);
    }
}
