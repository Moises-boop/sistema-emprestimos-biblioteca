namespace BibliotecaApp.Domain.Conference;

public class EtapaConferencia
{
    public string Nome { get; }
    public bool Obrigatoria { get; }

    // Construtor internal para reforçar a composição: em uso externo ao assembly,
    // a etapa deve ser criada pelo método EmprestimoMaterial.AdicionarEtapaConferencia().
    internal EtapaConferencia(string nome, bool obrigatoria)
    {
        Nome = string.IsNullOrWhiteSpace(nome)
            ? throw new ArgumentException("O nome da etapa de conferência é obrigatório.", nameof(nome))
            : nome;

        Obrigatoria = obrigatoria;
    }
}
