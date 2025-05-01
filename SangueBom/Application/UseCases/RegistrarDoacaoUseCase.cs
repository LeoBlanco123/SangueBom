using SangueBom.Domain.Entities;
using SangueBom.Domain.Interfaces;
using SangueBom.Domain.Repositories;
using SangueBom.Domain.Services;

namespace SangueBom.Application.UseCases
{
    public class RegistrarDoacaoUseCase
    {
        private readonly ICadastroDoadorRepositorio _doadorRepo;
        private readonly IDoacaoRepositorio _doacaoRepo;
        private readonly ValidadorDeDoacaoService _validador;

        public RegistrarDoacaoUseCase(
            ICadastroDoadorRepositorio doadorRepo,
            IDoacaoRepositorio doacaoRepo,
            ValidadorDeDoacaoService validador)
        {
            _doadorRepo = doadorRepo;
            _doacaoRepo = doacaoRepo;
            _validador = validador;
        }

        public async Task<string> ExecutarAsync(Guid doadorId, DateTime dataAtual)
        {
            var doador = await _doadorRepo.ObterPorIdAsync(doadorId);
            if (doador is null)
                return "Doador não encontrado.";

            var ultimaDoacao = await _doacaoRepo.ObterUltimaPorDoadorIdAsync(doadorId);

            if (!_validador.PodeRealizarDoacao(doador, ultimaDoacao?.DataDoacao, dataAtual))
                return "Doação não permitida por intervalo mínimo.";

            var novaDoacao = new Doacao(doadorId); // sem dataAtual

            await _doacaoRepo.RegistrarAsync(novaDoacao);

            return "Doação registrada com sucesso.";
        }
    }
}
