namespace BibliotecaApp.Domain.Entities;

public abstract class Entidade
{
    public Guid Id { get; }
    public string Codigo { get; }

    protected Entidade(string prefixo)
    {
        if (string.IsNullOrWhiteSpace(prefixo))
            throw new ArgumentException("O prefixo do código é obrigatório.", nameof(prefixo));

        Id = Guid.NewGuid();
        Codigo = $"{prefixo}-{Id.ToString("N")[..8].ToUpper()}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Entidade entidade && GetType() == entidade.GetType() && Id.Equals(entidade.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
