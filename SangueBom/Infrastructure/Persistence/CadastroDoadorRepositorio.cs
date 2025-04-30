using DoadoresDeSangue.Domain.ValueObjects;
using SangueBom.Domain.Entities;
using SangueBom.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SangueBom.Infrastructure.Repositories
{
    public class CadastroDoadorRepositorio : ICadastroDoadorRepositorio
    {
        // Repositório em memória para simulação
        private static readonly List<Doador> _doadores = new();

        public Task<Doador?> ObterPorCpfAsync(string cpf)
        {
            // Sempre limpar o CPF antes da comparação
            var cpfSemFormatacao = new CPF(cpf).Valor;

            var doador = _doadores.FirstOrDefault(d => d.Cpf.Valor == cpfSemFormatacao);
            return Task.FromResult(doador);
        }

        // Método para cadastrar um novo doador
        public Task CadastrarAsync(Doador doador)
        {
            _doadores.Add(doador);
            return Task.CompletedTask;
        }
    }
}
