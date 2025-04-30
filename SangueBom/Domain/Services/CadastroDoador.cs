// SangueBom.Application/Services/CadastroDoador.cs
using DoadoresDeSangue.Domain.ValueObjects;
using SangueBom.Domain.Entities;
using SangueBom.Domain.Enums;
using SangueBom.Domain.Interfaces;
using SangueBom.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace SangueBom.Application.Services
{
    public class CadastroDoador
    {
        private readonly ICadastroDoadorRepositorio _repositorio;
        public string Mensagem { get; private set; }

        public CadastroDoador(ICadastroDoadorRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task CadastrarDoadorAsync(
            string nome,
            string cpf,
            DateTime dataNascimento,
            Genero genero,
            TipoSanguineo tipoSanguineo,
            string rua,
            string numero,
            string bairro,
            string cidade,
            string estado,
            string telefone)
        {
            // 1) Cria e valida o objeto CPF (ele já normaliza)
            CPF cpfObj;
            try
            {
                cpfObj = new CPF(cpf); // Não precisa limpar manualmente, o ValueObject já faz isso
            }
            catch (ArgumentException)
            {
                Mensagem = "CPF inválido";
                return;
            }

            // 2) Verifica duplicidade com o CPF normalizado
            var existente = await _repositorio.ObterPorCpfAsync(cpfObj.Valor);
            if (existente != null)
            {
                Mensagem = "CPF já cadastrado";
                return;
            }

            // 3) Cadastra o doador
            var endereco = new Endereco(rua, numero, bairro, cidade, estado);
            var doador = new Doador(
                nome,
                cpfObj, // Usa o CPF já validado e limpo
                dataNascimento,
                genero,
                tipoSanguineo,
                endereco,
                telefone);

            await _repositorio.CadastrarAsync(doador);
            Mensagem = "Doador cadastrado com sucesso";
        }



    }
}
