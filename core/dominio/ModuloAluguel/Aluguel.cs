using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;
using System.Diagnostics;

namespace Locadora.Dominio.ModuloAluguel;
public class Aluguel : EntidadeBase<Aluguel>
{
    public Condutor? Condutor { get; set; }
    public Cliente? Cliente { get; set; }
    public Cobranca Cobranca { get; set; }
    public Funcionario? Funcionario { get; set; }
    public decimal Caucao = 1000;
    public Veiculo Veiculo { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public List<Taxa>? Taxas { get; set; }
    public float KmInicial { get; set; }
    public bool Status { get; set; }

    public Aluguel()
    {
        Taxas = new List<Taxa>();
    }

    public Aluguel(Condutor? condutor, Cliente? cliente, Cobranca cobranca, Funcionario? funcionario, Veiculo veiculo, DateTime dataSaida, DateTime dataRetornoPrevista, List<Taxa>? taxas, float kmInicial)
    {
        Id = Guid.NewGuid();
        Condutor = condutor;
        Cliente = cliente;
        Cobranca = cobranca;
        Funcionario = funcionario;
        Veiculo = veiculo;
        DataSaida = dataSaida;
        DataRetornoPrevista = dataRetornoPrevista;
        Taxas = taxas;
        KmInicial = veiculo.Kilometragem;
        Status = true;
    }

    public override void AtualizarRegistro(Aluguel registroEditado)
    {
        Condutor = registroEditado.Condutor;
        Cliente = registroEditado.Cliente;
        Cobranca = registroEditado.Cobranca;
        Caucao = registroEditado.Caucao;
        Veiculo = registroEditado.Veiculo;
        DataSaida = registroEditado.DataSaida;
        DataRetornoPrevista = registroEditado.DataRetornoPrevista;
        Taxas = registroEditado.Taxas;
        KmInicial = registroEditado.KmInicial;
    }

    public int CalcularDiarias()
    {
        if (DataRetornoPrevista == null)
            throw new InvalidOperationException("DataDevolucao must have a value to calculate diarias.");

        TimeSpan duracao =DataRetornoPrevista - DataSaida;
        int diarias = (int)Math.Ceiling(duracao.TotalDays);

        return diarias;
    }
    public int CalcularKms(float KmDevolucao)
    {
        var distanciaPercorrida = KmDevolucao - KmInicial;
        Math.Ceiling((decimal)distanciaPercorrida!);

        return (int)distanciaPercorrida;
    }

    public decimal CalculcarTotal(Devolucao devolucao)
    {
        decimal total = 0;

        if (Cobranca.PlanoCobranca == ModuloCobranca.PlanoCobrancaEnum.Diaria)
        {
            total = (decimal)Cobranca.PrecoDiaria! * CalcularDiarias();
            total += (decimal)Cobranca.PrecoKm! * CalcularKms(devolucao.KmDevolucao);
        }
        else if (Cobranca.PlanoCobranca == ModuloCobranca.PlanoCobrancaEnum.Controlado)
        {
            total = (decimal)Cobranca.PrecoDiaria! * CalcularDiarias();

            var kmsRodados = CalcularKms(devolucao.KmDevolucao);

            if(kmsRodados > Cobranca.KmDisponiveis)
            {
                var kmsExtrapolados = kmsRodados - Cobranca.KmDisponiveis;

                total += (decimal)(kmsExtrapolados * Cobranca.PrecoPorKmExtrapolado)!;
            }
        }
        else
        {
            total = (decimal)Cobranca.Taxa!;
        }

        if (!devolucao.TanqueCheio)
            total += CalcularCombustivel(devolucao.LitrosNaChegada);

        return total;
    }

    public decimal CalcularCombustivel(int LitrosNaChegada)
    {
        var combustivelRestante = Veiculo.CapacidadeCombustivel - LitrosNaChegada;
        
        return Veiculo.Combustivel.CalcularCombustivel(combustivelRestante);
    }
}
