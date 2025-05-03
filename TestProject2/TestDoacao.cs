using SangueBom.Domain.Entities;
using SangueBom.Domain.Enums;
using SangueBom.Domain.Services;
using DoadoresDeSangue.Domain.ValueObjects;
using Bunit;
using SangueBom.Domain.ValueObjects;
using Moq;
using SangueBom.Domain.Interfaces;
using SangueBom.Domain.Repositories;

namespace TestProject2
{
    public class TestDoacao
    {
        private readonly ValidadorDeDoacaoService _validador;
        private readonly IRepositorioDoacao _doacaoRepo;
        private readonly ICadastroDoadorRepositorio _doadorRepo;
        private readonly Mock<IDoacaoRepositorio> _doacaoRepoMock;

        public TestDoacao()
        {
            _validador = new ValidadorDeDoacaoService();
            _doacaoRepoMock = new Mock<IDoacaoRepositorio>();
        }


        private Doador CriarDoadorMasculino(string nome, DateTime dataNascimento)
        {
            var cpf = new CPF("529.235.898-31");
            var endereco = new Endereco("Rua A", "123", "Bairro B", "Cidade C", "Estado D");
            return new Doador(nome, cpf, dataNascimento, Genero.Masculino, TipoSanguineo.OPositivo, endereco, "11999999999");
        }

        private Doador CriarDoadoraFeminina(string nome, DateTime dataNascimento)
        {
            var cpf = new CPF("529.235.898-31");
            var endereco = new Endereco("Rua X", "456", "Bairro Y", "Cidade Z", "Estado W");
            return new Doador(nome, cpf, dataNascimento, Genero.Feminino, TipoSanguineo.ANegativo, endereco, "11888888888");
        }

        [Fact]
        public void DevePermitirNovaDoacaoParaHomem_ComIntervaloSuperiorA60Dias()
        {
            var doador = CriarDoadorMasculino("João da Silva", new DateTime(1990, 1, 1));
            var dataUltimaDoacao = DateTime.Today.AddDays(-61);
            var hoje = DateTime.Today;

            var resultado = _validador.PodeRealizarDoacao(doador, dataUltimaDoacao, hoje);

            Assert.True(resultado, "Homem com intervalo superior a 60 dias deve poder doar.");
        }

        [Fact]
        public void DeveBloquearNovaDoacaoParaHomem_ComIntervaloInferiorA60Dias()
        {
            var doador = CriarDoadorMasculino("Carlos Souza", new DateTime(1995, 5, 20));
            var dataUltimaDoacao = DateTime.Today.AddDays(-30);
            var hoje = DateTime.Today;

            var resultado = _validador.PodeRealizarDoacao(doador, dataUltimaDoacao, hoje);

            Assert.False(resultado, "Homem com intervalo inferior a 60 dias não deve poder doar.");
        }

        [Fact]
        public void DevePermitirNovaDoacaoParaMulher_ComIntervaloSuperiorA90Dias()
        {
            var doador = CriarDoadoraFeminina("Maria Oliveira", new DateTime(1988, 3, 15));
            var dataUltimaDoacao = DateTime.Today.AddDays(-91);
            var hoje = DateTime.Today;

            var resultado = _validador.PodeRealizarDoacao(doador, dataUltimaDoacao, hoje);

            Assert.True(resultado, "Mulher com intervalo superior a 90 dias deve poder doar.");
        }

        [Fact]
        public void DeveBloquearNovaDoacaoParaMulher_ComIntervaloInferiorA90Dias()
        {
            var doador = CriarDoadoraFeminina("Ana Lima", new DateTime(1993, 7, 10));
            var dataUltimaDoacao = DateTime.Today.AddDays(-45);
            var hoje = DateTime.Today;

            var resultado = _validador.PodeRealizarDoacao(doador, dataUltimaDoacao, hoje);

            Assert.False(resultado, "Mulher com intervalo inferior a 90 dias não deve poder doar.");
        }

        [Fact]
        public async Task TestarRegistrarNovaDoacao_SemDoacoes_Previa()
        {
            // Arrange
            var doadoraLarissa = new Doador(
                nome: "Larissa Matos",
                cpf: new CPF("529.235.898-31"),
                dataNascimento: new DateTime(1995, 08, 15),
                genero: Genero.Feminino,
                tipoSanguineo: TipoSanguineo.APositivo,
                endereco: new Endereco("Rua Y", "456", "Bairro X", "Cidade W", "Estado B"),
                telefone: "11999999999"
            );

            // Nenhuma doação registrada para Larissa
            var doacoesLarissa = new List<Doacao>();
            _doacaoRepoMock.Setup(repo => repo.ObterPorDoadorIdAsync(doadoraLarissa.Id)).ReturnsAsync(doacoesLarissa);

            // Simulando o comportamento de registrar a doação
            _doacaoRepoMock.Setup(repo => repo.RegistrarAsync(It.IsAny<Doacao>())).Returns(Task.CompletedTask);

            // Act - Registrar uma nova doação
            var novaDoacao = new Doacao(doadorId: doadoraLarissa.Id, dataDoacao: DateTime.Today);
            await _doacaoRepoMock.Object.RegistrarAsync(novaDoacao);

            // Assert
            _doacaoRepoMock.Verify(repo => repo.RegistrarAsync(It.IsAny<Doacao>()), Times.Once);
        }

        [Fact]
        public async Task TestarHistoricoDoacoes_DeveExibirDatasDeDoacoes()
        {
            // Arrange
            var doadorLucas = new Doador(
                nome: "Lucas Fernandes",
                cpf: new CPF("529.235.898-31"),
                dataNascimento: new DateTime(1990, 05, 10),
                genero: Genero.Masculino,
                tipoSanguineo: TipoSanguineo.OPositivo,
                endereco: new Endereco("Rua X", "123", "Bairro Y", "Cidade Z", "Estado A"),
                telefone: "11999999999"
            );

            var doacoesLucas = new List<Doacao>
            {
                new Doacao(doadorId: doadorLucas.Id, dataDoacao: new DateTime(2024, 01, 15)),
                new Doacao(doadorId: doadorLucas.Id, dataDoacao: new DateTime(2024, 02, 20)),
                new Doacao(doadorId: doadorLucas.Id, dataDoacao: new DateTime(2024, 03, 25))
            };

            // Configura o mock para retornar as doações de Lucas
            _doacaoRepoMock.Setup(repo => repo.ObterPorDoadorIdAsync(doadorLucas.Id)).ReturnsAsync(doacoesLucas);

            // Act
            var historicoDoacoes = await _doacaoRepoMock.Object.ObterPorDoadorIdAsync(doadorLucas.Id);

            // Assert
            Assert.Equal(3, historicoDoacoes.Count);
            Assert.Contains(historicoDoacoes, d => d.DataDoacao == new DateTime(2024, 01, 15));
            Assert.Contains(historicoDoacoes, d => d.DataDoacao == new DateTime(2024, 02, 20));
            Assert.Contains(historicoDoacoes, d => d.DataDoacao == new DateTime(2024, 03, 25));
        }
    }
}