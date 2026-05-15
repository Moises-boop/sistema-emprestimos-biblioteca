namespace BibliotecaApp.Domain.Entities;

public class RecursoComplementar : Entidade
{
    public string Descricao { get; }

    public RecursoComplementar(string descricao) : base("REC")
    {
        Descricao = string.IsNullOrWhiteSpace(descricao)
            ? throw new ArgumentException("A descrição é obrigatória.", nameof(descricao))
            : descricao;
    }
}
