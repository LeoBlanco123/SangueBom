using SangueBom.Domain.Entities;
using SangueBom.Domain.Interfaces;

namespace SangueBom.Infrastructure.Repositories
{
    public class CadastroDoadorRepositorio : ICadastroDoadorRepositorio
    {
        private readonly List<Doador> _doadores = new();

        public Task CadastrarAsync(Doador doador)
        {
            _doadores.Add(doador);
            return Task.CompletedTask;
        }

        public Task<List<Doador>> ListarAsync()
        {
            return Task.FromResult(_doadores.ToList());
        }

        public Task<Doador> ObterPorCpfAsync(string cpf)
        {
            var doador = _doadores.FirstOrDefault(d => d.Cpf.Valor == cpf);
            return Task.FromResult(doador);
        }

        public Task<Doador> ObterPorIdAsync(Guid doadorId)
        {
            var doador = _doadores.FirstOrDefault(d => d.Id == doadorId);
            return Task.FromResult(doador);
        }
    }
}
