using SangueBom.Domain.Entities;

namespace SangueBom.Domain.Repositories
{
    public interface IDoacaoRepository
    {
        Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId);
        Task AdicionarAsync(Doacao doacao);
    }
}

