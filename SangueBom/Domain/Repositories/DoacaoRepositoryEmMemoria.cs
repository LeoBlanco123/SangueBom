using SangueBom.Domain.Entities;
using SangueBom.Domain.Repositories;

namespace SangueBom.Infrastructure.Persistence
{
    public class DoacaoRepositoryEmMemoria : IDoacaoRepository
    {
        private readonly List<Doacao> _doacoes = new();

        public Task AdicionarAsync(Doacao doacao)
        {
            _doacoes.Add(doacao);
            return Task.CompletedTask;
        }

        public Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId)
        {
            var lista = _doacoes.Where(d => d.DoadorId == doadorId).ToList();
            return Task.FromResult(lista);
        }
    }
}















































































































































