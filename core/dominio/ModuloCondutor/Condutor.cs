namespace Locadora.Dominio.ModuloCondutor;
public class Condutor : EntidadeBase<Condutor>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Cpf { get; set; }
    public string Cnh { get; set; }
    public DateTime Validade { get; set; }

    public Condutor() { }

    public Condutor(string nome, string email, string telefone, string cpf, string cnh, DateTime validade)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cpf = cpf;
        Cnh = cnh;
        Validade = validade;
    }
    public override void AtualizarRegistro(Condutor registroEditado)
    {
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone = registroEditado.Telefone;
        Cpf = registroEditado.Cpf;
        Cnh = registroEditado.Cnh;
        Validade = registroEditado.Validade;
    }
}
