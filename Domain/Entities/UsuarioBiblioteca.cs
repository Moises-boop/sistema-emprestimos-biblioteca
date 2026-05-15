using BibliotecaApp.Domain.ValueObjects;
using BibliotecaApp.Infrastructure.Validation;

namespace BibliotecaApp.Domain.Entities;

public class UsuarioBiblioteca : Entidade
{
    public string Nome { get; private set; }
    public string? Email { get; private set; }
    public Endereco? Endereco { get; private set; }

    public UsuarioBiblioteca(string nome, string? email = null, Endereco? endereco = null)
        : base("USR")
    {
        Nome = string.IsNullOrWhiteSpace(nome)
            ? throw new ArgumentException("O nome do usuário é obrigatório.", nameof(nome))
            : nome;

        if (!string.IsNullOrWhiteSpace(email) && !RegularExpressions.IsValidEmail(email))
            throw new ArgumentException("Email inválido.", nameof(email));

        Email = email;
        Endereco = endereco;
    }

    public void AlterarNome(string novoNome)
    {
        Nome = string.IsNullOrWhiteSpace(novoNome)
            ? throw new ArgumentException("O nome do usuário é obrigatório.", nameof(novoNome))
            : novoNome;
    }

    public void AlterarEmail(string? novoEmail)
    {
        if (!string.IsNullOrWhiteSpace(novoEmail) && !RegularExpressions.IsValidEmail(novoEmail))
            throw new ArgumentException("Email inválido.", nameof(novoEmail));

        Email = novoEmail;
    }

    public void AlterarEndereco(Endereco? novoEndereco)
    {
        Endereco = novoEndereco;
    }
}
