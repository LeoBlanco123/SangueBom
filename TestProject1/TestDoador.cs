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
using DoadoresDeSangue.Domain.ValueObjects;
using SangueBom.Domain.ValueObjects;
using System.Security.Cryptography.Xml;


namespace TestProject1
{
    public class TestDoador : TestContext
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
            // Arrange: Simula repositório necessário
            Services.AddScoped<ICadastroDoadorRepositorio, CadastroDoadorRepositorio>();

            // Renderiza componente
            var cadastroComponent = RenderComponent<Cadastro>();

            // Assert: Verifica todos os campos
            Assert.NotNull(cadastroComponent.Find("input#nome"));
            Assert.NotNull(cadastroComponent.Find("input#cpf"));
            Assert.NotNull(cadastroComponent.Find("input#dataNascimento"));
            Assert.NotNull(cadastroComponent.Find("select#tipoSanguineo")); // ← Aqui falha se o select não renderizar
            Assert.NotNull(cadastroComponent.Find("input#rua"));
            Assert.NotNull(cadastroComponent.Find("input#numero"));
            Assert.NotNull(cadastroComponent.Find("input#bairro"));
            Assert.NotNull(cadastroComponent.Find("input#cidade"));
            Assert.NotNull(cadastroComponent.Find("input#estado"));
            Assert.NotNull(cadastroComponent.Find("select#genero"));
        }

        [Fact]
       

        public async Task AoPreencherCamposValidos_EFormularioEhSubmetido_DeveExibirMensagemDeSucesso()
        {
            // Arrange
            Services.AddScoped<ICadastroDoadorRepositorio, CadastroDoadorRepositorio>();

            var cut = RenderComponent<Cadastro>();

            // Act
            cut.Find("#nome").Change("João da Silva");
            cut.Find("#cpf").Change("935.411.347-80");
            cut.Find("#dataNascimento").Change("1990-01-01");
            cut.Find("#tipoSanguineo").Change("O+");
            cut.Find("#rua").Change("Rua das Flores");
            cut.Find("#numero").Change("123");
            cut.Find("#bairro").Change("Centro");
            cut.Find("#cidade").Change("São Paulo");
            cut.Find("#estado").Change("SP");
            cut.Find("#genero").Change("Masculino");

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


        [Fact]
        public void Deve_Calcular_Idade_Corretamente_AoCriarDoador()
        {
            // Arrange
            var dataNascimento = new DateTime(1990, 1, 1);
            var dataHoje = new DateTime(2025, 1, 1);
            var cpf = new CPF("935.411.347-80");
            var endereco = new Endereco("Rua X", "123", "Centro", "São Paulo", "SP");

            // Act
            var doador = new Doador(
                nome: "Carlos Silva",
                cpf: cpf,
                dataNascimento: dataNascimento,
                genero: Genero.Masculino,
                tipoSanguineo: TipoSanguineo.APositivo,
                endereco: endereco,
                telefone: "11999999999"
            );

            var idadeEsperada = 35;
            var idadeCalculada = ObterIdadeSimulada(dataNascimento, dataHoje);

            // Assert
            Assert.Equal(idadeEsperada, idadeCalculada);
            Assert.Equal(idadeEsperada, doador.Idade);
        }

        // Método auxiliar para simular o cálculo de idade como feito na entidade
        private int ObterIdadeSimulada(DateTime nascimento, DateTime dataReferencia)
        {
            var idade = dataReferencia.Year - nascimento.Year;
            if (nascimento.Date > dataReferencia.AddYears(-idade)) idade--;
            return idade;
        }


      
            [Fact]
            public void NaoDevePermitirCadastroDeMenorDe18Anos()
            {
                // Arrange
                var dataNascimentoMenor = new DateTime(2010, 1, 1); // idade: 15 anos em 2025
                var cpf = new CPF("935.411.347-80");
                var endereco = new Endereco("Rua X", "123", "Centro", "São Paulo", "SP");

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() =>
                    new Doador(
                        nome: "Pedro Menor",
                        cpf: cpf,
                        dataNascimento: dataNascimentoMenor,
                        genero: Genero.Masculino,
                        tipoSanguineo: TipoSanguineo.OPositivo,
                        endereco: endereco,
                        telefone: "11999999999"
                    )
                );

                Assert.Equal("Doador deve ter 18 anos ou mais.", ex.Message);
            }

        [Fact]
        public void CamposDevemTerTiposCorretosEFormatacao()
        {
            // Arrange: simula repositório injetável
            var mock = new Mock<ICadastroDoadorRepositorio>();
            Services.AddSingleton(mock.Object);

            var cut = RenderComponent<Cadastro>();

            // Assert: Verifica tipos e estrutura dos campos

            var nomeInput = cut.Find("input#nome");
            Assert.Equal("text", nomeInput.GetAttribute("type"));

            var cpfInput = cut.Find("input#cpf");
            Assert.Equal("text", cpfInput.GetAttribute("type"));

            var dataInput = cut.Find("input#dataNascimento");
            Assert.Equal("date", dataInput.GetAttribute("type"));

            var tipoSanguineoSelect = cut.Find("select#tipoSanguineo");
            var opcoes = tipoSanguineoSelect.Children;
            Assert.Contains(opcoes, opt => opt.TextContent.Contains("A+"));
            Assert.Contains(opcoes, opt => opt.TextContent.Contains("O-"));

            var generoSelect = cut.Find("select#genero");
            Assert.NotNull(generoSelect);

            var telefoneInput = cut.Find("input#telefone");
            Assert.Equal("tel", telefoneInput.GetAttribute("type")); 
        }
    }
}

    

