using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Locadora.Dominio.ModuloCliente;
public class Cliente : EntidadeBase<Cliente>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public TipoClienteEnum TipoCliente { get; set; }
    public string? CPF { get; set; }
    public string? CNPJ { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }

    public Cliente()
    {
    }

    public Cliente(string nome, string email, string telefone, TipoClienteEnum tipoCliente, string? cPF, string? cNPJ, string estado, string cidade, string bairro, string rua, string numero)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Telefone = telefone;
        TipoCliente = tipoCliente;
        CPF = cPF;
        CNPJ = cNPJ;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        Rua = rua;
        Numero = numero;
    }

    public override void AtualizarRegistro(Cliente registroEditado)
    {
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone = registroEditado.Telefone;
        TipoCliente = registroEditado.TipoCliente;
        CPF = registroEditado.CPF;
        CNPJ = registroEditado.CNPJ;
        Estado = registroEditado.Estado;
        Cidade = registroEditado.Cidade;
        Bairro = registroEditado.Bairro;
        Rua = registroEditado.Rua;
        Numero = registroEditado.Numero;
    }
}
