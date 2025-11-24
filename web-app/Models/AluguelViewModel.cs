using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Locadora.WebApp.Models;

public class FormularioAluguelViewModel
{
    public Guid? Condutor { get; set; }
    [Required(ErrorMessage = "O campo cliente é obrigatório.")]
    public Guid Cliente { get; set; }
    [Required(ErrorMessage = "O campo cobrança é obrigatório")]
    public Guid Cobranca { get; set; }
    [Required(ErrorMessage = "O campo veículo é obrigatório.")]
    public Guid Veiculo { get; set; }
    [Required(ErrorMessage = "O campo de data de saída é obrigatório")]
    public DateTime DataSaida { get; set; }
    [Required(ErrorMessage = "O campo de data de retorno prevista é obrigatório.")]
    public DateTime DataRetornoPrevista { get; set; }
    public List<Guid>? Taxas { get; set; }
    [Required(ErrorMessage = "O campo Km inicial é obrigatório")]
    public float KmInicial { get; set; }

    public List<SelectListItem>? CondutoresDisponiveis { get; set; }
    public List<SelectListItem>? ClientesDisponivies { get; set; }
    public List<SelectListItem>? VeiculosDisponiveis { get; set; }
    public List<SelectListItem>? CobrancasDisponivies { get; set; }
    public List<SelectListItem>? TaxasDisponiveis { get; set; }

    public static Aluguel ParaEntidade(
        FormularioAluguelViewModel viewModel,
        List<Condutor> condutores,
        List<Cliente> clientes,
        List<Veiculo> veiculos,
        List<Cobranca> cobrancas,
        List<Taxa> taxas
        )
    {
        var condutorSelecionado = condutores.Find(c => c.Id == viewModel.Condutor) ?? throw new ArgumentException("Condutor inválido.");
        var clienteSelecionado = clientes.Find(c => c.Id == viewModel.Cliente) ?? throw new ArgumentException("Cliente inválido.");
        var cobrancaSelecionada = cobrancas.Find(c => c.Id == viewModel.Cobranca) ?? throw new ArgumentException("Cobrança inválida.");
        var veiculoSelecionado = veiculos.Find(v => v.Id == viewModel.Veiculo) ?? throw new ArgumentException("Veículo inválido.");
        var taxasSelecionadas = taxas
            .Where(t => viewModel.Taxas != null && viewModel.Taxas.Contains(t.Id))
            .ToList();

        return new Aluguel(
            condutorSelecionado,
            clienteSelecionado,
            cobrancaSelecionada,
            veiculoSelecionado,
            viewModel.DataSaida,
            viewModel.DataRetornoPrevista,
            taxasSelecionadas,
            viewModel.KmInicial
            );
    }
}

public class CadastrarAluguelViewModel : FormularioAluguelViewModel
{
    public CadastrarAluguelViewModel() { }

    public CadastrarAluguelViewModel(
        List<Condutor> condutores,
        List<Cliente> clientes,
        List<Veiculo> veiculos,
        List<Cobranca> cobrancas,
        List<Taxa> taxas)
    {
        CondutoresDisponiveis = condutores
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();

        ClientesDisponivies = clientes
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();

        VeiculosDisponiveis = veiculos
            .Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Modelo
            }).ToList();

        CobrancasDisponivies = cobrancas
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.PlanoCobranca.ToString()
            }).ToList();

        TaxasDisponiveis = taxas
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Descricao
            }).ToList();
    }
}

public class EditarAluguelViewModel : FormularioAluguelViewModel
{
    //public Guid Id { get; set; }

    //public EditarAluguelViewModel() { }

    //public EditarAluguelViewModel(
    //    Aluguel aluguel,
    //    List<Condutor> condutores,
    //    List<Cliente> clientes,
    //    List<Veiculo> veiculos,
    //    List<Cobranca> cobrancas,
    //    List<Taxa> taxas
    //    )
    //{
    //    Id = aluguel.Id;
    //    Cliente = aluguel.Cliente.Id;
    //    Cobranca = aluguel.Cobranca.Id;
    //    Caucao = aluguel.Caucao;
    //    Veiculo = aluguel.Veiculo.Id;
    //    DataSaida = aluguel.DataSaida;
    //    DataRetornoPrevista = aluguel.DataRetornoPrevista;
    //    DataDevolucao = aluguel.DataDevolucao;
    //    Taxas = aluguel.Taxas?.Select(t => t.Id).ToList();
    //    KmInicial = aluguel.KmInicial;
    //    KmDevolucao = aluguel?.KmDevolucao;
    //    TanqueCheio = aluguel?.TanqueCheio;
    //    Status = aluguel.Status;
    //    Total = aluguel.Total;

    //    CondutoresDisponiveis = condutores
    //    .Select(c => new SelectListItem
    //    {
    //        Value = c.Id.ToString(),
    //        Text = c.Nome
    //    }).ToList();

    //    ClientesDisponivies = clientes
    //        .Select(c => new SelectListItem
    //        {
    //            Value = c.Id.ToString(),
    //            Text = c.Nome
    //        }).ToList();

    //    VeiculosDisponiveis = veiculos
    //        .Select(v => new SelectListItem
    //        {
    //            Value = v.Id.ToString(),
    //            Text = v.Modelo
    //        }).ToList();

    //    CobrancasDisponivies = cobrancas
    //        .Select(c => new SelectListItem
    //        {
    //            Value = c.Id.ToString(),
    //            Text = c.PlanoCobranca.ToString()
    //        }).ToList();

    //    TaxasDisponiveis = taxas
    //        .Select(t => new SelectListItem
    //        {
    //            Value = t.Id.ToString(),
    //            Text = t.Descricao
    //        }).ToList();
    //}
}
public class ExcluirAluguelViewModel
{
    public Guid Id { get; set; }
    public Condutor Condutor { get; set; }

    public ExcluirAluguelViewModel(Aluguel aluguel)
    {
        Id = aluguel.Id;
        Condutor = aluguel.Condutor;
    }
}

public class VisualizarAluguelViewModel
{
    public List<DetalhesAluguelViewModel> Alugueis { get; set; }

    public VisualizarAluguelViewModel(List<Aluguel> alugueis)
    {
        Alugueis = alugueis
            .Select(c => DetalhesAluguelViewModel.ParaDetalhesVm(c))
            .ToList();
    }
}

public class DetalhesAluguelViewModel
{
    public Guid Id { get; set; }
    public Condutor? Condutor { get; set; }
    public Cliente? Cliente { get; set; }
    public Cobranca Cobranca { get; set; }
    public decimal Caucao { get; set; }
    public Veiculo Veiculo { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public List<Taxa>? Taxas { get; set; }
    public float KmInicial { get; set; }
    public float? KmDevolucao { get; set; }
    public bool? TanqueCheio { get; set; }
    public bool Status { get; set; }
    public decimal Total { get; set; }

    public static DetalhesAluguelViewModel ParaDetalhesVm(Aluguel aluguel)
    {
        return new DetalhesAluguelViewModel
        {
            Id = aluguel.Id,
            Condutor = aluguel.Condutor,
            Cliente = aluguel.Cliente,
            Cobranca = aluguel.Cobranca,
            Caucao = aluguel.Caucao,
            Veiculo = aluguel.Veiculo,
            DataSaida = aluguel.DataSaida,
            DataRetornoPrevista = aluguel.DataRetornoPrevista,
            DataDevolucao = aluguel.DataDevolucao,
            Taxas = aluguel.Taxas,
            KmInicial = aluguel.KmInicial,
            KmDevolucao = aluguel.KmDevolucao,
            TanqueCheio = aluguel.TanqueCheio,
            Status = aluguel.Status,
            Total = aluguel.Total,

        };
    }
}
