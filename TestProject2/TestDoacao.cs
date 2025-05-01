using SangueBom.Domain.Entities;
using SangueBom.Domain.Enums;
using SangueBom.Domain.Services;
using SangueBom.Domain.ValueObjects;
using DoadoresDeSangue.Domain.ValueObjects;
using Bunit;
using SangueBom.Application.UseCases;
using SangueBom.Infrastructure.Repositories;

namespace TestProject2
{
    public class TestDoacao : TestContext
    {
        [Fact]
        public void Deve_permitir_doacao_masculina_quando_passaram_mais_de_60_dias()
        {
            // Arrange
            var validador = new ValidadorDeDoacaoService();

            var cpf = new CPF("41931921806"); // ou CPF.Create("12345678900") se for um método de fábrica
            var endereco = new Endereco("Rua A", "123", "Bairro B", "Cidade C", "Estado D");

            var doador = new Doador(
                nome: "João",
                cpf: cpf,
                dataNascimento: new DateTime(1990, 1, 1),
                genero: Genero.Masculino,
                tipoSanguineo: TipoSanguineo.OPositivo, // ajuste conforme seu enum
                endereco: endereco,
                telefone: "11999999999"
            );

            var ultimaDoacao = DateTime.Today.AddDays(-61);
            var hoje = DateTime.Today;

            // Act
            var resultado = validador.PodeRealizarDoacao(doador, ultimaDoacao, hoje);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void Nao_deve_permitir_doacao_masculina_quando_passaram_menos_de_60_dias()
        {
            // Arrange
            var validador = new ValidadorDeDoacaoService();
            var cpf = new CPF("41931921806"); // ou CPF.Create("12345678900") se for um método de fábrica
            var endereco = new Endereco("Rua A", "123", "Bairro B", "Cidade C", "Estado D");

            var doador = new Doador(
                nome: "João",
                cpf: cpf,
                dataNascimento: new DateTime(1990, 1, 1),
                genero: Genero.Masculino,
                tipoSanguineo: TipoSanguineo.OPositivo, // ajuste conforme seu enum
                endereco: endereco,
                telefone: "11999999999"
            );
            var ultimaDoacao = DateTime.Today.AddDays(-30);
            var hoje = DateTime.Today;

            // Act
            var resultado = validador.PodeRealizarDoacao(doador, ultimaDoacao, hoje);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void Deve_permitir_doacao_feminina_quando_passaram_mais_de_90_dias()
        {
            // Arrange
            var validador = new ValidadorDeDoacaoService();

            var cpf = new CPF("41931921806"); // ou CPF.Create("12345678900") se for um método de fábrica
            var endereco = new Endereco("Rua A", "123", "Bairro B", "Cidade C", "Estado D");

            var doador = new Doador(
                nome: "Ana",
                cpf: cpf,
                dataNascimento: new DateTime(1990, 1, 1),
                genero: Genero.Feminino,
                tipoSanguineo: TipoSanguineo.OPositivo, // ajuste conforme seu enum
                endereco: endereco,
                telefone: "11999999999"
            );

            var ultimaDoacao = DateTime.Today.AddDays(-91);
            var hoje = DateTime.Today;

            // Act
            var resultado = validador.PodeRealizarDoacao(doador, ultimaDoacao, hoje);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void Nao_deve_permitir_doacao_feminina_quando_passaram_menos_de_90_dias()
        {
            // Arrange
            var validador = new ValidadorDeDoacaoService();
            var cpf = new CPF("41931921806"); // ou CPF.Create("12345678900") se for um método de fábrica
            var endereco = new Endereco("Rua A", "123", "Bairro B", "Cidade C", "Estado D");

            var doador = new Doador(
                nome: "Ana",
                cpf: cpf,
                dataNascimento: new DateTime(1990, 1, 1),
                genero: Genero.Feminino,
                tipoSanguineo: TipoSanguineo.OPositivo, // ajuste conforme seu enum
                endereco: endereco,
                telefone: "11999999999"
            );
            var ultimaDoacao = DateTime.Today.AddDays(-45);
            var hoje = DateTime.Today;

            // Act
            var resultado = validador.PodeRealizarDoacao(doador, ultimaDoacao, hoje);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_exibir_tres_doacoes_para_LucasFernandes()
        {
            // Arrange
            var doadorId = Guid.NewGuid();
            var doacaoRepo = new RepositorioDoacaoEmMemoria();

            var doacoes = new List<Doacao>
        {
            new Doacao(doadorId, new DateTime(2023, 1, 10)),
            new Doacao(doadorId, new DateTime(2023, 4, 10)),
            new Doacao(doadorId, new DateTime(2023, 7, 10))
        };

            foreach (var doacao in doacoes)
            {
                await doacaoRepo.RegistrarAsync(doacao);
            }

            // Act
            var historico = await doacaoRepo.ObterPorDoadorIdAsync(doadorId);

            // Assert
            Assert.Equal(3, historico.Count);
            Assert.Contains(historico, d => d.DataDoacao == new DateTime(2023, 1, 10));
            Assert.Contains(historico, d => d.DataDoacao == new DateTime(2023, 4, 10));
            Assert.Contains(historico, d => d.DataDoacao == new DateTime(2023, 7, 10));
        }

        [Fact]
        public async Task Scenario6_RegistroDeDoacaoSemHistoricoAnterior()
        {
            // Arrange
            var doadorRepositorio = new CadastroDoadorRepositorio();
            var doacaoRepositorio = new RepositorioDoacaoEmMemoria();
            var validador = new ValidadorDeDoacaoService();
            var cpf = new CPF("41931921806");

            var doadora = new Doador(
                nome: "Larissa Matos",
                cpf: cpf,
                dataNascimento: new DateTime(1995, 5, 10),
                genero: Genero.Feminino,
                tipoSanguineo: TipoSanguineo.APositivo,
                endereco: new SangueBom.Domain.ValueObjects.Endereco("Rua A", "123", "Centro", "Cidade", "Estado"),
                telefone: "11999999999"
            );

            await doadorRepositorio.CadastrarAsync(doadora);

            // Act
            var doadorRecuperado = await doadorRepositorio.ObterPorCpfAsync("41931921806");

            var novaDoacao = new Doacao(doadorRecuperado.Id, DateTime.Today);
            await doacaoRepositorio.RegistrarAsync(novaDoacao);

            // Assert
            var doacoes = await doacaoRepositorio.ObterPorDoadorIdAsync(doadorRecuperado.Id);
            Assert.Single(doacoes);
            Assert.Equal(DateTime.Today, doacoes[0].DataDoacao);
        }


    }
}
