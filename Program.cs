using BibliotecaApp.Domain.Conference;
using BibliotecaApp.Domain.Entities;
using BibliotecaApp.Domain.ValueObjects;

Console.WriteLine("=== Sistema de Empréstimos de Biblioteca ===\n");

static void ExecutarTentativaInvalida(string titulo, Action acao)
{
    Console.WriteLine($"[Teste inválido] {titulo}");

    try
    {
        acao();
        Console.WriteLine("ERRO: a operação deveria ter falhado, mas foi aceita.\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exceção capturada corretamente: {ex.Message}\n");
    }
}

var enderecoUsuario = new Endereco(
    rua: "Rua das Acácias",
    numero: "120",
    bairro: "Centro",
    cidade: "Medianeira",
    estado: "PR",
    cep: "85884-000");

var enderecoUnidade = new Endereco(
    rua: "Avenida Brasil",
    numero: "500",
    bairro: "Centro",
    cidade: "Medianeira",
    estado: "PR",
    cep: "85884-000");

var usuario = new UsuarioBiblioteca(
    nome: "Moises Grassi",
    email: "moises.grassi@email.com",
    endereco: enderecoUsuario);

var autor = new Autor("Noam Chomsky");

var material = new MaterialBiblioteca(
    descricao: "Linguagem e Mente",
    autor: autor,
    isbn: "9788578279710");

var unidade = new UnidadeBiblioteca(
    nome: "Biblioteca UTFPR - Campus Medianeira",
    endereco: enderecoUnidade);

var supervisor = new Atendente(
    nome: "Marta Loewenstein Grassi",
    endereco: enderecoUnidade,
    email: "marta.loewenstein@email.com");

var atendenteResponsavel = new Atendente(
    nome: "Ruan Pereira",
    endereco: enderecoUnidade,
    email: "ruan.pereira@email.com");

var atendenteNaoResponsavel = new Atendente(
    nome: "Atendente Externo",
    endereco: enderecoUnidade,
    email: "externo@email.com");

var leitorDigital = new RecursoComplementar("Leitor digital institucional");
var foneOuvido = new RecursoComplementar("Fone de ouvido para material audiovisual");
var caboHdmi = new RecursoComplementar("Cabo HDMI reserva");

var emprestimo = new EmprestimoMaterial(
    solicitante: usuario,
    materialPrincipal: material,
    unidade: unidade);

emprestimo.DefinirSupervisor(supervisor);
emprestimo.AdicionarAtendenteResponsavel(atendenteResponsavel);
emprestimo.AdicionarRecursoComplementar(leitorDigital);
emprestimo.AdicionarRecursoComplementar(foneOuvido);

var etapaEstadoFisico = emprestimo.AdicionarEtapaConferencia(
    descricao: "Conferir estado físico do material",
    obrigatoria: true);

var etapaDataDevolucao = emprestimo.AdicionarEtapaConferencia(
    descricao: "Registrar data prevista de devolução",
    obrigatoria: true);

var etapaRecursos = emprestimo.AdicionarEtapaConferencia(
    descricao: "Conferir recursos complementares vinculados",
    obrigatoria: false);

ExecutarTentativaInvalida(
    "Adicionar atendente responsável duplicado",
    () => emprestimo.AdicionarAtendenteResponsavel(atendenteResponsavel));

ExecutarTentativaInvalida(
    "Adicionar o supervisor como atendente responsável",
    () => emprestimo.AdicionarAtendenteResponsavel(supervisor));

ExecutarTentativaInvalida(
    "Adicionar recurso complementar duplicado",
    () => emprestimo.AdicionarRecursoComplementar(leitorDigital));

ExecutarTentativaInvalida(
    "Remover recurso complementar não associado ao empréstimo",
    () => emprestimo.RemoverRecursoComplementar(caboHdmi));

ExecutarTentativaInvalida(
    "Registrar parecer com atendente que não é responsável pelo empréstimo",
    () => emprestimo.RegistrarParecer(
        atendenteNaoResponsavel,
        etapaEstadoFisico,
        "Tentativa inválida de parecer.",
        aprovado: true));

// A criação direta abaixo só é possível dentro deste mesmo projeto porque o construtor é internal.
// Em uso externo ao assembly, a etapa deve ser criada pelo método AdicionarEtapaConferencia().
var etapaForaDoEmprestimo = new EtapaConferencia("Etapa criada fora do empréstimo", true);

ExecutarTentativaInvalida(
    "Registrar parecer para etapa que não pertence ao empréstimo",
    () => emprestimo.RegistrarParecer(
        atendenteResponsavel,
        etapaForaDoEmprestimo,
        "Tentativa inválida de parecer.",
        aprovado: true));

ExecutarTentativaInvalida(
    "Calcular percentual antes da finalização",
    () => Console.WriteLine(emprestimo.CalcularPercentualDeAprovacaoDeConferencias()));

emprestimo.RegistrarParecer(
    atendenteResponsavel,
    etapaEstadoFisico,
    "Material conferido sem danos aparentes.",
    aprovado: true);

ExecutarTentativaInvalida(
    "Finalizar empréstimo sem parecer em todas as etapas obrigatórias",
    () => emprestimo.Finalizar());

emprestimo.RegistrarParecer(
    atendenteResponsavel,
    etapaDataDevolucao,
    "Data prevista de devolução registrada.",
    aprovado: true);

emprestimo.RegistrarParecer(
    atendenteResponsavel,
    etapaRecursos,
    "Recursos complementares conferidos com pendência leve.",
    aprovado: false);

emprestimo.Finalizar();

var percentual = emprestimo.CalcularPercentualDeAprovacaoDeConferencias();

Console.WriteLine("=== Empréstimo finalizado com sucesso ===");
Console.WriteLine($"Código do empréstimo: {emprestimo.Codigo}");
Console.WriteLine($"Solicitante: {emprestimo.Solicitante.Nome} ({emprestimo.Solicitante.Codigo})");
Console.WriteLine($"Material: {emprestimo.MaterialPrincipal.Descricao} ({emprestimo.MaterialPrincipal.Codigo})");
Console.WriteLine($"Unidade: {emprestimo.Unidade.Nome} ({emprestimo.Unidade.Codigo})");
Console.WriteLine($"Percentual de conferências aprovadas: {percentual:F2}%\n");

ExecutarTentativaInvalida(
    "Adicionar etapa depois do empréstimo finalizado",
    () => emprestimo.AdicionarEtapaConferencia("Etapa posterior à finalização", true));

ExecutarTentativaInvalida(
    "Criar usuário com nome vazio",
    () => new UsuarioBiblioteca("", "usuario@email.com", enderecoUsuario));
