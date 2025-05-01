using SangueBom.Domain.Entities;
using SangueBom.Domain.Repositories;

public class RepositorioDoacaoEmMemoria : IRepositorioDoacao
{
    private readonly List<Doacao> _doacoes = new();

    public Task<List<Doacao>> ObterPorDoadorIdAsync(Guid doadorId)
    {
        var resultado = _doacoes.Where(d => d.DoadorId == doadorId).ToList();
        return Task.FromResult(resultado);
    }

    public Task RegistrarAsync(Doacao doacao)
    {
        _doacoes.Add(doacao);
        return Task.CompletedTask;
    }
}
