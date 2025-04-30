// TestProject1/UnitTest2.cs

using SangueBom.Domain.Entities;
using SangueBom.Domain.Enums;
using SangueBom.Domain.Repositories;
using SangueBom.Domain.Services;
using SangueBom.Domain.ValueObjects;
using SangueBom.Application.UseCases;
using Xunit;
using DoadoresDeSangue.Domain.ValueObjects;

namespace TestProject1.UseCases
{
    public class UnitTest2
    {
        private readonly ValidadorDeDoacaoService _service = new();

        private Doador CriarLucasFernandes()
        {
            return new Doador(
                nome: "Lucas Fernandes",
                cpf: new CPF("12345678909"),
                DateTime.Today.AddYears(-30),
                Genero.Masculino,
                TipoSanguineo.APositivo,
                new Endereco("Rua X", "123", "Centro", "SP", "SP"),
                "11999999999"
            );
        }

        private Doador CriarLarissaMatos()
        {
            return new Doador(
                nome: "Larissa Matos",
                cpf: new CPF("98765432100"),
                DateTime.Today.AddYears(-25),
                Genero.Feminino,
                TipoSanguineo.OPositivo,
                new Endereco("Rua Y", "456", "Bairro Z", "RJ", "RJ"),
                "21999999999"
            );
        }

        private Doador CriarDoadorMasculino()
        {
            return new Doador(
                nome: "João",
                cpf: new CPF("12345678909"),
                dataNascimento: DateTime.Today.AddYears(-30),
                genero: Genero.Masculino,
                tipoSanguineo: TipoSanguineo.OPositivo,
                endereco: new Endereco("Rua A", "10", "Centro", "São Paulo", "SP"),
                telefone: "11999999999"
            );
        }

        private Doador CriarDoadoraFeminina()
        {
            return new Doador(
                nome: "Maria",
                cpf: new CPF("98765432100"),
                dataNascimento: DateTime.Today.AddYears(-28),
                genero: Genero.Feminino,
                tipoSanguineo: TipoSanguineo.APositivo,
                endereco: new Endereco("Rua B", "20", "Bairro Y", "Rio de Janeiro", "RJ"),
                telefone: "21999999999"
            );
        }

        [Fact]
        public void Deve_permitir_doacao_masculina_quando_passaram_mais_de_60_dias()
        {
            var doador = CriarDoadorMasculino();
            var ultimaDoacao = DateTime.Today.AddDays(-61);
            var hoje = DateTime.Today;

            var resultado = _service.PodeRealizarDoacao(doador, ultimaDoacao, hoje);

            Assert.True(resultado);
        }

        [Fact]
        public void Nao_deve_permitir_doacao_masculina_quando_passaram_menos_de_60_dias()
        {
            var doador = CriarDoadorMasculino();
            var ultimaDoacao = DateTime.Today.AddDays(-30);
            var hoje = DateTime.Today;

            var resultado = _service.PodeRealizarDoacao(doador, ultimaDoacao, hoje);

            Assert.False(resultado);
        }

        [Fact]
        public void Deve_permitir_doacao_feminina_quando_passaram_mais_de_90_dias()
        {
            var doadora = CriarDoadoraFeminina();
            var ultimaDoacao = DateTime.Today.AddDays(-91);
            var hoje = DateTime.Today;

            var resultado = _service.PodeRealizarDoacao(doadora, ultimaDoacao, hoje);

            Assert.True(resultado);
        }

        [Fact]
        public void Nao_deve_permitir_doacao_feminina_quando_passaram_menos_de_90_dias()
        {
            var doadora = CriarDoadoraFeminina();
            var ultimaDoacao = DateTime.Today.AddDays(-45);
            var hoje = DateTime.Today;

            var resultado = _service.PodeRealizarDoacao(doadora, ultimaDoacao, hoje);

            Assert.False(resultado);
        }

        [Fact(Skip = "Aguardando integração com repositório real de doadores")]
        public async Task Deve_registrar_primeira_doacao_para_LarissaMatos()
        {
            // TODO: Usar repositório real injetado no Program.cs
            /*
            var doadoraId = Guid.Parse("ID-REAL-DA-LARISSA");
            var useCase = new RegistrarDoacaoUseCase(realDoadorRepo, realDoacaoRepo, new ValidadorDeDoacaoService());

            var resultado = await useCase.ExecutarAsync(doadoraId, DateTime.Today);

            Assert.Equal("Doação registrada com sucesso.", resultado);
            */
        }

        [Fact(Skip = "Aguardando integração com repositório real de doadores")]
        public async Task Deve_exibir_tres_doacoes_para_LucasFernandes()
        {
            // TODO: Usar ID real e repositório real implementado pela equipe de doadores
            /*
            var doadorId = Guid.Parse("ID-REAL-AQUI");
            var useCase = new RegistrarDoacaoUseCase(realDoadorRepo, realDoacaoRepo, new ValidadorDeDoacaoService());

            var historico = await realDoacaoRepo.ObterPorDoadorIdAsync(doadorId);

            Assert.Equal(3, historico.Count);
            Assert.Contains(historico, d => d.Data == new DateTime(2023, 1, 10));
            Assert.Contains(historico, d => d.Data == new DateTime(2023, 4, 10));
            Assert.Contains(historico, d => d.Data == new DateTime(2023, 7, 10));
            */
        }
    }
}