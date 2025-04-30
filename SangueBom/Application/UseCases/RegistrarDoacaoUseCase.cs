// Application/UseCases/RegistrarDoacaoUseCase.cs
using SangueBom.Domain.Entities;
using SangueBom.Domain.Repositories;
using SangueBom.Domain.Services;

namespace SangueBom.Application.UseCases
{
    public class RegistrarDoacaoUseCase
    {
        //private readonly IDoadorRepository _doadorRepository;
        private readonly IDoacaoRepository _doacaoRepository;
        private readonly ValidadorDeDoacaoService _validador;

        // 🔧 Esse construtor já prepara a injeção para amanhã, quando o repositório de doadores estiver pronto.
        public RegistrarDoacaoUseCase(
           // IDoadorRepository doadorRepository, // 💡 FALTA IMPLEMENTAR NA EQUIPE DE DOADORES
            IDoacaoRepository doacaoRepository,
            ValidadorDeDoacaoService validador)
        {
            //_doadorRepository = doadorRepository;
            _doacaoRepository = doacaoRepository;
            _validador = validador;
        }

        public async Task<string> ExecutarAsync(Guid doadorId, DateTime dataDoacao)
        {
            // 🔴 Aguardando integração com a parte de doadores
            // var doador = await _doadorRepository.ObterPorIdAsync(doadorId);
            // if (doador is null)
            //     return "Doador não encontrado.";

            // 🔧 TEMPORÁRIO: simular doador apenas para compilação (REMOVER AMANHÃ)
            Doador doador = null!; // ⚠️ Substituir por dado real assim que integrar

            // var historico = await _doacaoRepository.ObterPorDoadorIdAsync(doadorId);
            // var ultimaDoacao = historico.OrderByDescending(d => d.Data).FirstOrDefault()?.Data;

            // if (!_validador.PodeRealizarDoacao(doador, ultimaDoacao, dataDoacao))
            // {
            //     return $"Doação não permitida. Intervalo mínimo não respeitado para {doador.Genero}.";
            // }

            var novaDoacao = new Doacao(doadorId, dataDoacao);
            await _doacaoRepository.AdicionarAsync(novaDoacao);

            return "Doação registrada (modo isolado). Integrar com doador amanhã.";
        }
    }
}
