using BibliotecaApp.Domain.Conference;

namespace BibliotecaApp.Domain.Entities;

public class EmprestimoMaterial : Entidade
{
    public UsuarioBiblioteca Solicitante { get; }
    public MaterialBiblioteca MaterialPrincipal { get; }
    public UnidadeBiblioteca Unidade { get; }
    public Atendente? AtendenteSupervisor { get; private set; }
    public bool Finalizado { get; private set; }

    private readonly List<Atendente> _atendentesResponsaveis = new();
    private readonly List<RecursoComplementar> _recursosComplementares = new();
    private readonly List<ParecerConferencia> _pareceres = new();
    private readonly List<EtapaConferencia> _etapasConferencia = new();

    public IReadOnlyCollection<Atendente> AtendentesResponsaveis => _atendentesResponsaveis;
    public IReadOnlyCollection<RecursoComplementar> RecursosComplementares => _recursosComplementares;
    public IReadOnlyCollection<ParecerConferencia> Pareceres => _pareceres;
    public IReadOnlyCollection<EtapaConferencia> EtapasConferencia => _etapasConferencia;

    public EmprestimoMaterial(
        UsuarioBiblioteca solicitante,
        MaterialBiblioteca materialPrincipal,
        UnidadeBiblioteca unidade,
        Atendente? atendenteSupervisor = null) : base("EMP")
    {
        Solicitante = solicitante ?? throw new ArgumentNullException(nameof(solicitante), "O solicitante é obrigatório.");
        MaterialPrincipal = materialPrincipal ?? throw new ArgumentNullException(nameof(materialPrincipal), "O material principal é obrigatório.");
        Unidade = unidade ?? throw new ArgumentNullException(nameof(unidade), "A unidade é obrigatória.");
        AtendenteSupervisor = atendenteSupervisor;
    }

    public void DefinirSupervisor(Atendente supervisor)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível definir supervisor em um empréstimo finalizado.");

        if (supervisor == null)
            throw new ArgumentNullException(nameof(supervisor), "O supervisor é obrigatório.");

        if (_atendentesResponsaveis.Any(a => a.Equals(supervisor)))
            throw new InvalidOperationException("O supervisor não pode ser um atendente responsável pelo mesmo empréstimo.");

        AtendenteSupervisor = supervisor;
    }

    public void AdicionarAtendenteResponsavel(Atendente atendente)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível adicionar atendentes a um empréstimo finalizado.");

        if (atendente == null)
            throw new ArgumentNullException(nameof(atendente), "O atendente é obrigatório.");

        if (AtendenteSupervisor != null && AtendenteSupervisor.Equals(atendente))
            throw new InvalidOperationException("O supervisor não pode ser adicionado como atendente responsável.");

        if (_atendentesResponsaveis.Any(a => a.Equals(atendente)))
            throw new ArgumentException("Este atendente já está adicionado.", nameof(atendente));

        _atendentesResponsaveis.Add(atendente);
    }

    public void RemoverAtendenteResponsavel(Atendente atendente)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível remover atendentes de um empréstimo finalizado.");

        if (atendente == null)
            throw new ArgumentNullException(nameof(atendente), "O atendente é obrigatório.");

        if (!_atendentesResponsaveis.Any(a => a.Equals(atendente)))
            throw new ArgumentException("Este atendente não está adicionado.", nameof(atendente));

        _atendentesResponsaveis.Remove(atendente);
    }

    public void AdicionarRecursoComplementar(RecursoComplementar recurso)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível adicionar recursos a um empréstimo finalizado.");

        if (recurso == null)
            throw new ArgumentNullException(nameof(recurso), "O recurso é obrigatório.");

        if (_recursosComplementares.Any(r => r.Equals(recurso)))
            throw new ArgumentException("Este recurso já está adicionado.", nameof(recurso));

        _recursosComplementares.Add(recurso);
    }

    public void RemoverRecursoComplementar(RecursoComplementar recurso)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível remover recursos de um empréstimo finalizado.");

        if (recurso == null)
            throw new ArgumentNullException(nameof(recurso), "O recurso é obrigatório.");

        if (!_recursosComplementares.Any(r => r.Equals(recurso)))
            throw new ArgumentException("Este recurso não está adicionado.", nameof(recurso));

        _recursosComplementares.Remove(recurso);
    }

    public EtapaConferencia AdicionarEtapaConferencia(string descricao, bool obrigatoria)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível adicionar etapas a um empréstimo finalizado.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição é obrigatória.", nameof(descricao));

        if (_etapasConferencia.Any(e => e.Nome.Equals(descricao, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException("Já existe uma etapa de conferência com esta descrição.", nameof(descricao));

        var etapa = new EtapaConferencia(descricao, obrigatoria);
        _etapasConferencia.Add(etapa);

        return etapa;
    }

    public void RegistrarParecer(Atendente atendente, EtapaConferencia etapa, string descricao, bool aprovado)
    {
        if (Finalizado)
            throw new InvalidOperationException("Não é possível registrar pareceres em um empréstimo finalizado.");

        if (atendente == null)
            throw new ArgumentNullException(nameof(atendente), "O atendente é obrigatório.");

        if (etapa == null)
            throw new ArgumentNullException(nameof(etapa), "A etapa é obrigatória.");

        if (!_atendentesResponsaveis.Any(a => a.Equals(atendente)))
            throw new ArgumentException("O atendente deve ser um dos responsáveis pelo empréstimo.", nameof(atendente));

        if (!_etapasConferencia.Any(e => e.Equals(etapa)))
            throw new ArgumentException("A etapa de conferência não pertence a este empréstimo.", nameof(etapa));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição do parecer é obrigatória.", nameof(descricao));

        if (_pareceres.Any(p =>
            p.AtendenteResponsavel.Equals(atendente) &&
            p.EtapaConferencia.Equals(etapa)))
        {
            throw new ArgumentException("Este atendente já registrou parecer para esta etapa.");
        }

        var parecer = new ParecerConferencia(atendente, etapa, descricao, aprovado);
        _pareceres.Add(parecer);
    }

    public void Finalizar()
    {
        if (Finalizado)
            throw new InvalidOperationException("Este empréstimo já está finalizado.");

        if (_atendentesResponsaveis.Count == 0)
            throw new InvalidOperationException("É necessário pelo menos um atendente responsável para finalizar o empréstimo.");

        if (!_etapasConferencia.Any(e => e.Obrigatoria))
            throw new InvalidOperationException("É necessário pelo menos uma etapa obrigatória para finalizar o empréstimo.");

        if (_etapasConferencia
            .Where(e => e.Obrigatoria)
            .Any(e => !_pareceres.Any(p => p.EtapaConferencia.Equals(e))))
        {
            throw new InvalidOperationException("Todas as etapas obrigatórias devem ter parecer registrado.");
        }

        if (AtendenteSupervisor != null &&
            _atendentesResponsaveis.Any(a => a.Equals(AtendenteSupervisor)))
        {
            throw new InvalidOperationException("O atendente supervisor não pode ser um dos responsáveis pelo empréstimo.");
        }

        Finalizado = true;
    }

    public double CalcularPercentualDeAprovacaoDeConferencias()
    {
        if (!Finalizado)
            throw new InvalidOperationException("O percentual só pode ser calculado após a finalização do empréstimo.");

        if (_etapasConferencia.Count == 0)
            return 0;

        var etapasAprovadas = _etapasConferencia
            .Count(etapa => _pareceres.Any(p =>
                p.EtapaConferencia.Equals(etapa) &&
                p.Aprovado));

        return (double)etapasAprovadas / _etapasConferencia.Count * 100;
    }
}
