using Locadora.Dominio.Autenticacao;

namespace Locadora.Dominio.ModuloFuncionario;
public class Funcionario : EntidadeBase<Funcionario>
{
    public string Nome { get; set; }
    public DateTime DataAdmissao { get; set; }
    public decimal Salario { get; set; }
    public bool EstaAtivo { get; set; } = true;
    public Guid UserId { get; set; }
    public User User { get; set; } = new User();

    public Funcionario() { }

    public Funcionario(string nome, DateTime dataAdmissao, decimal salario) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        DataAdmissao = dataAdmissao;
        Salario = salario;
    }

    public void AssociateUser(User user)
    {
        this.User = user;
        this.UserId = user.Id;
    }

    public void AssociateTenant(Guid tenantId) => this.EmpresaId = tenantId;

    public void Deactivate() => this.EstaAtivo = false;

    public override void AtualizarRegistro(Funcionario registroEditado)
    {
        Nome = registroEditado.Nome;
        DataAdmissao = registroEditado.DataAdmissao;
        Salario = registroEditado.Salario;
    }
}
