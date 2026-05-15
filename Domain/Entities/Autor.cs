namespace BibliotecaApp.Domain.Entities;

public class Autor : Entidade
{
    public string Nome { get; }

    public Autor(string nome) : base("AUT")
    {
        Nome = string.IsNullOrWhiteSpace(nome)
            ? throw new ArgumentException("O nome do autor é obrigatório.", nameof(nome))
            : nome;
    }
}
