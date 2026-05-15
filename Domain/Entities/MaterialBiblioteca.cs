using BibliotecaApp.Infrastructure.Validation;

namespace BibliotecaApp.Domain.Entities;

public class MaterialBiblioteca : Entidade
{
    public string Descricao { get; }
    public Autor Autor { get; }
    public string? Isbn { get; }

    public MaterialBiblioteca(string descricao, Autor autor, string? isbn = null) : base("MAT")
    {
        Descricao = string.IsNullOrWhiteSpace(descricao)
            ? throw new ArgumentException("A descrição é obrigatória.", nameof(descricao))
            : descricao;

        Autor = autor ?? throw new ArgumentNullException(nameof(autor), "O autor é obrigatório.");

        if (!string.IsNullOrWhiteSpace(isbn) && !RegularExpressions.IsValidIsbn(isbn))
            throw new ArgumentException("ISBN inválido. Use ISBN-10 ou ISBN-13 sem hífens.", nameof(isbn));

        Isbn = isbn;
    }
}
