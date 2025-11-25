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
    public decimal Caucao { get; set; }
    public Guid Veiculo { get; set; }
    [Required(ErrorMessage = "O campo de data de saída é obrigatório")]
    public DateTime DataSaida { get; set; }
    [Required(ErrorMessage = "O campo de data de retorno prevista é obrigatório.")]
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime DataDevolucao { get; set; }
    public List<Guid>? Taxas { get; set; }
    [Required(ErrorMessage = "O campo Km inicial é obrigatório")]
    public float KmInicial { get; set; }
    public float KmDevolucao { get; set; }
    public bool TanqueCheio { get; set; }
    public bool Status { get; set; }
    public bool SeguroAcionado { get; set; }
    public decimal Total { get; set; }

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
        var condutorSelecionado = condutores.Find(c => c.Id == viewModel.Condutor);
        var clienteSelecionado = clientes.Find(c => c.Id == viewModel.Cliente);
        var cobrancaSelecionada = cobrancas.Find(c => c.Id == viewModel.Cobranca);
        var veiculoSelecionado = veiculos.Find(v => v.Id == viewModel.Veiculo);
        var taxasSelecionadas = taxas
            .Where(t => viewModel.Taxas != null && viewModel.Taxas.Contains(t.Id))
            .ToList();

        return new Aluguel(
            condutorSelecionado,
            clienteSelecionado,
            cobrancaSelecionada,
            viewModel.Caucao,
            veiculoSelecionado,
            viewModel.DataSaida,
            viewModel.DataRetornoPrevista,
            viewModel.DataDevolucao,
            taxasSelecionadas,
            viewModel.KmInicial,
            viewModel.KmDevolucao,
            viewModel.TanqueCheio,
            viewModel.Status,
            viewModel.SeguroAcionado,
            viewModel.Total
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
    public Guid Id { get; set; }

    public EditarAluguelViewModel() { }

    public EditarAluguelViewModel(
        Aluguel aluguel,
        List<Condutor> condutores,
        List<Cliente> clientes,
        List<Veiculo> veiculos,
        List<Cobranca> cobrancas,
        List<Taxa> taxas)
    {
        Id = aluguel.Id;
        Condutor = aluguel.Condutor?.Id;
        Cliente = aluguel.Cliente?.Id ?? Guid.Empty;
        Cobranca = aluguel.Cobranca?.Id ?? Guid.Empty;
        Caucao = aluguel.Caucao;
        Veiculo = aluguel.Veiculo?.Id ?? Guid.Empty;
        DataSaida = aluguel.DataSaida;
        DataRetornoPrevista = aluguel.DataRetornoPrevista;
        DataDevolucao = aluguel.DataDevolucao ?? default;
        Taxas = aluguel.Taxas?.Select(t => t.Id).ToList() ?? new List<Guid>();
        KmInicial = aluguel.KmInicial;
        KmDevolucao = aluguel.KmDevolucao ?? default;
        TanqueCheio = aluguel.TanqueCheio ?? default;
        Status = aluguel.Status;
        SeguroAcionado = aluguel.SeguroAcionado;
        Total = aluguel.Total;

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

public class DevolucaoAluguelViewModel
{
    public Guid Id { get; set; }

    public DateTime DataDevolucao { get; set; }
    public float KmDevolucao { get; set; }
    public bool TanqueCheio { get; set; }
    public bool SeguroAcionado { get; set; }
    public decimal Total { get; set; }

    public DevolucaoAluguelViewModel() { }


    public DevolucaoAluguelViewModel(Guid id, DateTime dataDevolucao, float kmDevolucao, bool tanqueCheio, bool seguroAcionado, decimal total)
    {
        Id = id;
        DataDevolucao = dataDevolucao;
        KmDevolucao = kmDevolucao;
        TanqueCheio = tanqueCheio;
        SeguroAcionado = seguroAcionado;
        Total = total;
    }
}

public class ExcluirAluguelViewModel
{
    public Guid Id { get; set; }
    public Condutor Condutor { get; set; }

    public ExcluirAluguelViewModel() { }

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
