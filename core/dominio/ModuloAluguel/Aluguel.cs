using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;

namespace Locadora.Dominio.ModuloAluguel;
public class Aluguel : EntidadeBase<Aluguel>
{
    public Condutor Condutor { get; set; }
    public Cliente? Cliente { get; set; }
    public Cobranca Cobranca { get; set; }
    public decimal Caucao { get; set; } = 1000;
    public Veiculo Veiculo { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime DataRetornoPrevista { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public List<Taxa>? Taxas { get; set; }
    public float KmInicial { get; set; }
    public float? KmDevolucao { get; set; }
    public bool? TanqueCheio { get; set; }
    public bool Status {  get; set; }

    public Aluguel()
    {
        Taxas = new List<Taxa>();
    }

    public Aluguel(Condutor condutor, Cliente? cliente, Cobranca cobranca, decimal caucao, Veiculo veiculo, DateTime dataSaida, DateTime dataRetornoPrevista, DateTime? dataDevolucao, List<Taxa>? taxas, float kmInicial, float? kmDevolucao, bool? tanqueCheio, bool status) : this()
    {
        Id = Guid.NewGuid();
        Condutor = condutor;
        Cliente = cliente;
        Cobranca = cobranca;
        Caucao = caucao;
        Veiculo = veiculo;
        DataSaida = dataSaida;
        DataRetornoPrevista = dataRetornoPrevista;
        DataDevolucao = dataDevolucao;
        Taxas = taxas;
        KmInicial = kmInicial;
        KmDevolucao = kmDevolucao;
        TanqueCheio = tanqueCheio;
        Status = status;
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
        DataDevolucao = registroEditado.DataDevolucao;
        Taxas = registroEditado.Taxas;
        KmInicial = registroEditado.KmInicial;
        KmDevolucao = registroEditado.KmDevolucao;
        TanqueCheio = registroEditado.TanqueCheio;
    }
}
