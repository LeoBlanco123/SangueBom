using SangueBom.Domain.Entities;
using SangueBom.Domain.Interfaces;
using SangueBom.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RepositorioDoacaoEmMemoria : IRepositorioDoacao
{
    private readonly List<Doacao> _doacoes = new();

    public Task RegistrarAsync(Doacao doacao)
    {
        _doacoes.Add(doacao);
        return Task.CompletedTask;
    }

    public Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId)
    {
        var doacoesDoDoador = _doacoes.Where(d => d.DoadorId == doadorId).ToList();
        return Task.FromResult(doacoesDoDoador);
    }
}
