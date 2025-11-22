namespace Locadora.Dominio.ModuloFuncionario;
public class Funcionario : EntidadeBase<Funcionario>
{
    public string Nome { get; set; }
    public DateTime DataAdmissao { get; set; }
    public decimal Salario { get; set; }

    public Funcionario() { }

    public Funcionario(string nome, DateTime dataAdmissao, decimal salario)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        DataAdmissao = dataAdmissao;
        Salario = salario;
    }
    public override void AtualizarRegistro(Funcionario registroEditado)
    {
        Nome = registroEditado.Nome;
        DataAdmissao = registroEditado.DataAdmissao;
        Salario = registroEditado.Salario;
    }
}
