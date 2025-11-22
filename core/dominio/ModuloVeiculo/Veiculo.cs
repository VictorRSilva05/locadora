using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloGrupoVeiculo;

namespace Locadora.Dominio.ModuloVeiculo;
public class Veiculo : EntidadeBase<Veiculo>
{
    public byte[]? Foto { get; set; }
    public GrupoVeiculo GrupoVeiculo { get; set; }
    public string Marca { get; set; }
    public string Cor { get; set; }
    public string Modelo { get; set; }
    public Combustivel Combustivel { get; set; }
    public int CapacidadeCombustivel { get; set; }
    public int Ano { get; set; }

    public Veiculo() { }

    public Veiculo(byte[]? foto, GrupoVeiculo grupoVeiculo, string marca, string cor, string modelo, Combustivel combustivel, int capacidadeCombustivel, int ano)
    {
        Id = Guid.NewGuid();
        Foto = foto;
        GrupoVeiculo = grupoVeiculo;
        Marca = marca;
        Cor = cor;
        Modelo = modelo;
        Combustivel = combustivel;
        CapacidadeCombustivel = capacidadeCombustivel;
        Ano = ano;
    }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        Foto = registroEditado.Foto;
        GrupoVeiculo = registroEditado.GrupoVeiculo;
        Marca = registroEditado.Marca;
        Cor = registroEditado.Cor;
        Modelo = registroEditado.Modelo;
        Combustivel = registroEditado.Combustivel;
        CapacidadeCombustivel = CapacidadeCombustivel;
        Ano = registroEditado.Ano;
    }
}
