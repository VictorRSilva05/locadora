namespace Locadora.Dominio.ModuloTaxa;
public class Taxa : EntidadeBase<Taxa>
{
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public PlanoCobrancaEnum PlanoCobranca { get; set; }

    public Taxa()
    {
    }

    public Taxa(string descricao, decimal valor, PlanoCobrancaEnum planoCobranca)
    {
        Id = Guid.NewGuid();
        Descricao = descricao;
        Valor = valor;
        PlanoCobranca = planoCobranca;
    }
    public override void AtualizarRegistro(Taxa registroEditado)
    {
        Descricao = registroEditado.Descricao;
        Valor = registroEditado.Valor;
        PlanoCobranca = registroEditado.PlanoCobranca; 
    }
}
