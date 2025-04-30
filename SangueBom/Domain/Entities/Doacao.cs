public class Doacao
{
    public Guid Id { get; private set; }
    public Guid DoadorId { get; private set; }
    public DateTime Data { get; private set; }

    public Doacao(Guid doadorId, DateTime data)
    {
        Id = Guid.NewGuid();
        DoadorId = doadorId;
        Data = data;
    }
}