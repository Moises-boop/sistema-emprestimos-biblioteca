using BibliotecaApp.Domain.ValueObjects;

namespace BibliotecaApp.Domain.Entities;

public class UnidadeBiblioteca : Entidade
{
    public string Nome { get; private set; }
    public Endereco Endereco { get; private set; }

    public UnidadeBiblioteca(string nome, Endereco endereco)
        : base("UND")
    {
        Nome = string.IsNullOrWhiteSpace(nome)
            ? throw new ArgumentException("O nome da unidade é obrigatório.", nameof(nome))
            : nome;

        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco), "O endereço da unidade é obrigatório.");
    }

    public void AlterarNome(string novoNome)
    {
        Nome = string.IsNullOrWhiteSpace(novoNome)
            ? throw new ArgumentException("O nome da unidade é obrigatório.", nameof(novoNome))
            : novoNome;
    }

    public void AlterarEndereco(Endereco novoEndereco)
    {
        Endereco = novoEndereco ?? throw new ArgumentNullException(nameof(novoEndereco), "O endereço da unidade é obrigatório.");
    }
}
