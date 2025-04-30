namespace SangueBom.Domain.Entities
{   
    public class Doacao
    {
        public Guid Id { get; private set; }
        public Guid DoadorId { get; private set; }
        public DateTime DataDoacao { get; private set; }

        public Doacao(Guid doadorId)
        {
            Id = Guid.NewGuid();
            DoadorId = doadorId;
            DataDoacao = DateTime.Today;
        }
    }
}
