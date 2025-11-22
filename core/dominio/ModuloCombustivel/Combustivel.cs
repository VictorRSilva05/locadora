namespace Locadora.Dominio.ModuloCombustivel;
public class Combustivel : EntidadeBase<Combustivel>
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }

    public Combustivel() { }

    public  Combustivel(string nome, decimal preco)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Preco = preco;
    }
    public override void AtualizarRegistro(Combustivel registroEditado)
    {
        Nome = registroEditado.Nome;
        Preco = registroEditado.Preco;
    }
}
