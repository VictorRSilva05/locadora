
using Locadora.Dominio.ModuloFuncionario;

namespace Locadora.Dominio.ModuloAluguel;
public class Devolucao : EntidadeBase<Devolucao>
{
    public DateTime DataDevolucao { get; set; }
    public float KmDevolucao { get; set; }
    public int LitrosNaChegada { get; set; }
    public bool SeguroAcionado { get; set; }
    public bool TanqueCheio { get; set; }
    public decimal Total { get; set; }
    public Aluguel Aluguel { get; set; }

    public Devolucao() { }
    public Devolucao(DateTime dataDevolucao, float kmDevolucao, int litrosNaChegada, bool seguroAcionado, decimal total, bool tanqueCheio, Aluguel aluguel)
    {
        Id = Guid.NewGuid();
        DataDevolucao = dataDevolucao;
        KmDevolucao = kmDevolucao;
        LitrosNaChegada = litrosNaChegada;
        SeguroAcionado = seguroAcionado;
        Total = total;
        TanqueCheio = tanqueCheio;
        Aluguel = aluguel;
    }

    public override void AtualizarRegistro(Devolucao registroEditado)
    {
        throw new NotImplementedException();
    }
}
