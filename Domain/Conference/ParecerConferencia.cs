using AtendenteEntity = BibliotecaApp.Domain.Entities.Atendente;

namespace BibliotecaApp.Domain.Conference;

public record ParecerConferencia
{
    public AtendenteEntity AtendenteResponsavel { get; }
    public EtapaConferencia EtapaConferencia { get; }
    public string Observacao { get; }
    public bool Aprovado { get; }

    // Construtor internal para reforçar a composição: em uso externo ao assembly,
    // o parecer deve ser registrado pelo método EmprestimoMaterial.RegistrarParecer().
    internal ParecerConferencia(AtendenteEntity atendenteResponsavel, EtapaConferencia etapaConferencia, string observacao, bool aprovado)
    {
        AtendenteResponsavel = atendenteResponsavel ?? throw new ArgumentNullException(nameof(atendenteResponsavel), "O atendente responsável é obrigatório.");
        EtapaConferencia = etapaConferencia ?? throw new ArgumentNullException(nameof(etapaConferencia), "A etapa de conferência é obrigatória.");

        Observacao = string.IsNullOrWhiteSpace(observacao)
            ? throw new ArgumentException("Observação sobre a conferência é obrigatória.", nameof(observacao))
            : observacao;

        Aprovado = aprovado;
    }
}
