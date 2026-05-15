using BibliotecaApp.Infrastructure.Validation;

namespace BibliotecaApp.Domain.ValueObjects;

public record Endereco
{
    public string Rua { get; }
    public string Numero { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string Cep { get; }

    public Endereco(string rua, string numero, string bairro, string cidade, string estado, string cep)
    {
        Rua = string.IsNullOrWhiteSpace(rua)
            ? throw new ArgumentException("A rua é obrigatória.", nameof(rua))
            : rua;

        Numero = string.IsNullOrWhiteSpace(numero)
            ? throw new ArgumentException("O número é obrigatório.", nameof(numero))
            : numero;

        Bairro = string.IsNullOrWhiteSpace(bairro)
            ? throw new ArgumentException("O bairro é obrigatório.", nameof(bairro))
            : bairro;

        Cidade = string.IsNullOrWhiteSpace(cidade)
            ? throw new ArgumentException("A cidade é obrigatória.", nameof(cidade))
            : cidade;

        Estado = string.IsNullOrWhiteSpace(estado)
            ? throw new ArgumentException("O estado é obrigatório.", nameof(estado))
            : estado;

        Cep = RegularExpressions.IsValidCep(cep)
            ? cep
            : throw new ArgumentException("Formato de CEP inválido. Use o formato 00000-000.", nameof(cep));
    }
}
