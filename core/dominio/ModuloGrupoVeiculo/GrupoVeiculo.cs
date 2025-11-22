namespace Locadora.Dominio.ModuloGrupoVeiculo;
public class GrupoVeiculo : EntidadeBase<GrupoVeiculo>
{
    public string Nome { get; set; }

    public GrupoVeiculo() { }

    public GrupoVeiculo(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }
    public override void AtualizarRegistro(GrupoVeiculo registroEditado)
    {
        Nome = registroEditado.Nome;    
    }
}
