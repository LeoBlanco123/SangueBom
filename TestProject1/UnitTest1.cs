using Bunit;
using Xunit;
using SangueBom.Components.Pages;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SangueBom.Domain.Interfaces;
using SangueBom.Domain.Entities;
using SangueBom.Infrastructure.Repositories;
using SangueBom.Domain.Enums;
using SangueBom.Application.Services;

namespace TestProject1
{
    public class UnitTest1 : TestContext
    {
        [Fact]
        public void ClicarNoBotaoCadastrarNovoDoador_DeveRedirecionarParaTelaDeCadastro()
        {
            // Arrange: Renderiza o componente Home
            var homeComponent = RenderComponent<Home>();

            // Act: Simula o clique no botão "Cadastrar novo doador"
            homeComponent.Find("button").Click();

            // Assert: Verifica se a navegação foi para a página de cadastro
            var navigationManager = Services.GetRequiredService<FakeNavigationManager>();
            Assert.Equal("http://localhost/cadastro", navigationManager.Uri);
        }

        [Fact]
        public void DeveExibirTodosOsCamposDoFormularioDeCadastro()
        {
            // Arrange: Renderiza o componente Cadastro
            var cadastroComponent = RenderComponent<Cadastro>();

            // Act & Assert: Verifica se os campos estão presentes
            Assert.NotNull(cadastroComponent.Find("input#nome")); // Campo Nome
            Assert.NotNull(cadastroComponent.Find("input#cpf")); // Campo CPF
            Assert.NotNull(cadastroComponent.Find("input#dataNascimento")); // Campo Data de Nascimento
            Assert.NotNull(cadastroComponent.Find("select#tipoSanguineo")); // Campo Tipo Sanguíneo
            Assert.NotNull(cadastroComponent.Find("input#rua")); // Campo Rua (Endereço)
            Assert.NotNull(cadastroComponent.Find("input#numero")); // Campo Número (Endereço)
            Assert.NotNull(cadastroComponent.Find("input#bairro")); // Campo Bairro (Endereço)
            Assert.NotNull(cadastroComponent.Find("input#cidade")); // Campo Cidade (Endereço)
            Assert.NotNull(cadastroComponent.Find("input#estado")); // Campo Estado (Endereço)
            Assert.NotNull(cadastroComponent.Find("select#genero")); // Campo Gênero
        }

        [Fact]
        public async Task AoPreencherCamposValidos_EFormularioEhSubmetido_DeveExibirMensagemDeSucesso()
        {
            // Arrange
            var cut = RenderComponent<Cadastro>();

            // Act
            cut.Find("#nome").Change("João da Silva");
            cut.Find("#cpf").Change("123.456.789-09");
            cut.Find("#dataNascimento").Change("1990-01-01");
            cut.Find("#tipoSanguineo").Change("O+");
            cut.Find("#rua").Change("Rua das Flores");
            cut.Find("#numero").Change("123");
            cut.Find("#bairro").Change("Centro");
            cut.Find("#cidade").Change("São Paulo");
            cut.Find("#estado").Change("SP");
            cut.Find("#genero").Change("Masculino");

            // Clica no botão (aguarda async)
            await cut.InvokeAsync(() => cut.Find("button").Click());

            // Assert
            var mensagem = cut.Find("p").TextContent;
            Assert.Equal("Doador cadastrado com sucesso: João da Silva", mensagem);
        }

        [Fact]
        public async Task CadastroDoador_DeveExibirMensagemDeErro_QuandoCPFInvalido()
        {
            // Arrange
            var repositorio = new CadastroDoadorRepositorio();
            var servico = new CadastroDoador(repositorio);

            // Dados de teste
            string nome = "João da Silva";
            string cpfInvalido = "000.000.000-00";
            DateTime dataNascimento = new DateTime(2000, 1, 1);
            var genero = Genero.Masculino;
            var tipoSanguineo = TipoSanguineo.OPositivo;
            string rua = "Rua A";
            string numero = "123";
            string bairro = "Centro";
            string cidade = "São Paulo";
            string estado = "SP";
            string telefone = "11999999999";

            // Act
            await servico.CadastrarDoadorAsync(
                nome,
                cpfInvalido,
                dataNascimento,
                genero,
                tipoSanguineo,
                rua,
                numero,
                bairro,
                cidade,
                estado,
                telefone);

            // Assert
            Assert.Equal("CPF inválido", servico.Mensagem);
        }

        [Fact]
        public async Task CadastroDoador_DeveExibirMensagemDeErro_QuandoCPFDuplicado()
        {
            // Arrange
            var repositorio = new CadastroDoadorRepositorio();
            var servico = new CadastroDoador(repositorio);

            // Dados de teste
            string cpfExistente = "123.456.789-09";
            DateTime data1 = new DateTime(1990, 1, 1);
            DateTime data2 = new DateTime(1995, 5, 15);
            var genero = Genero.Feminino;
            var tipo = TipoSanguineo.APositivo;
            string rua = "Rua A", numero = "100", bairro = "Jardim", cidade = "Campinas", estado = "SP", tel = "11988888888";

            // Primeiro cadastro — deve ter sucesso
            await servico.CadastrarDoadorAsync(
                "Primeiro Doador",
                cpfExistente,
                data1,
                genero,
                tipo,
                rua,
                numero,
                bairro,
                cidade,
                estado,
                tel);
            Assert.Equal("Doador cadastrado com sucesso", servico.Mensagem);

            // Segundo cadastro com mesmo CPF — deve falhar
            await servico.CadastrarDoadorAsync(
                "Segundo Doador",
                cpfExistente,
                data2,
                genero,
                tipo,
                rua,
                numero,
                bairro,
                cidade,
                estado,
                tel);

            // Assert
            Assert.Equal("CPF já cadastrado", servico.Mensagem);
        }
    }
}


