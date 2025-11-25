using Locadora.Dominio.ModuloGrupoVeiculo;

namespace Locadora.Dominio.ModuloCobranca;
public class Cobranca : EntidadeBase<Cobranca>
{
    public GrupoVeiculo GrupoVeiculo { get; set; }
    public PlanoCobrancaEnum PlanoCobranca { get; set; }
    public decimal? PrecoDiaria { get; set; }
    public decimal? PrecoKm { get; set; }
    public int? KmDisponiveis { get; set; }
    public decimal? PrecoPorKmExtrapolado { get; set; }
    public decimal? Taxa { get; set; }

    public Cobranca()
    {
    }

    public Cobranca(GrupoVeiculo grupoVeiculo, PlanoCobrancaEnum planoCobranca, decimal? precoDiaria, decimal? precoKm, int? kmDisponiveis, decimal? precoPorKmExtrapolado, decimal? taxa) : this()
    {
        Id = Guid.NewGuid();
        GrupoVeiculo = grupoVeiculo;
        PlanoCobranca = planoCobranca;
        PrecoDiaria = precoDiaria;
        PrecoKm = precoKm;
        KmDisponiveis = kmDisponiveis;
        PrecoPorKmExtrapolado = precoPorKmExtrapolado;
        Taxa = taxa;
    }

    public override void AtualizarRegistro(Cobranca registroEditado)
    {
        GrupoVeiculo = registroEditado.GrupoVeiculo;
        PlanoCobranca = registroEditado.PlanoCobranca;
        PrecoDiaria = registroEditado.PrecoDiaria;
        PrecoKm = registroEditado.PrecoKm;
        KmDisponiveis = registroEditado.KmDisponiveis;
        PrecoPorKmExtrapolado = registroEditado.PrecoPorKmExtrapolado;
        Taxa = registroEditado.Taxa;
    }

    public decimal CalcularPlanoDiaria(int dias, int kms)
    {
        decimal total = 0;

        decimal precoDiaria = dias * (decimal)PrecoDiaria!;

        decimal precoPorKm = kms * (decimal)PrecoKm!;

        total = precoDiaria + precoPorKm;

        return total;
    }

    public decimal CalcularPlanoControlado(int dias,int kms)
    {
        decimal total = 0;

        decimal precoDiaria = dias * (decimal)PrecoDiaria!;

        decimal precoKmRodado = 0;

        int kmsExtrapolados = (int)KmDisponiveis! - kms;

        if(kmsExtrapolados > 0)
            precoKmRodado = kmsExtrapolados * (decimal)PrecoPorKmExtrapolado!;

        total =  precoDiaria + precoKmRodado;

        return total;
    }

}
