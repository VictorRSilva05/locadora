using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Locadora.Dominio.ModuloCliente;
public class Cliente : EntidadeBase<Cliente>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public TipoClienteEnum TipoCliente { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Bairro { get; set; }
    public string Rua { get; set; }
    public string Numero { get; set; }

    // Pessoa Física
    public string? CPF { get; set; }
    public string? RG { get; set; }
    public string? CNH { get; set; }
    public Cliente? PJ { get; set; }

    // Pessoa Jurídica
    public string? CNPJ { get; set; }
    public List<Cliente>? PF { get; set; } = new List<Cliente>();


    public Cliente()
    {
    }

    public Cliente(string nome, string email, string telefone, TipoClienteEnum tipoCliente, string estado, string cidade, string bairro, string rua, string numero, string? cPF, string? rG, string? cNH, Cliente? pJ)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        TipoCliente = tipoCliente;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        Rua = rua;
        Numero = numero;
        CPF = cPF;
        RG = rG;
        CNH = cNH;
        PJ = pJ;
    }

    public Cliente(string nome, string email, string telefone, TipoClienteEnum tipoCliente, string estado, string cidade, string bairro, string rua, string numero, string? cNPJ, List<Cliente>? pF)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        TipoCliente = tipoCliente;
        Estado = estado;
        Cidade = cidade;
        Bairro = bairro;
        Rua = rua;
        Numero = numero;
        CNPJ = cNPJ;
        PF = pF;
    }

    public override void AtualizarRegistro(Cliente registroEditado)
    {
        Nome = registroEditado.Nome;
        Email = registroEditado.Email;
        Telefone = registroEditado.Telefone;
        TipoCliente = registroEditado.TipoCliente;
        Estado = registroEditado.Estado;
        Cidade = registroEditado.Cidade;
        Bairro = registroEditado.Bairro;
        Rua = registroEditado.Rua;
        Numero = registroEditado.Numero;
        CPF = registroEditado.CPF;
        RG = registroEditado.RG;
        CNH = registroEditado.CNH;
        PJ = registroEditado.PJ;
        CNPJ = registroEditado.CNPJ;
        PF = registroEditado.PF;
    }
}
