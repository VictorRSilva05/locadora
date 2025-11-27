using FluentResults;
using Locadora.Aplicacao.Compartilhado;
using Locadora.Dominio.Autenticacao;
using Microsoft.AspNetCore.Identity;
namespace Locadora.Aplicacao.ModuloAutenticacao;
public class AutenticacaoAppService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    RoleManager<Role> roleManager
)
{
    private readonly UserManager<User> userManager = userManager;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly RoleManager<Role> roleManager = roleManager;

    public async Task<Result> RegistrarAsync(User usuario, string senha, Roles tipo)
    {
        var usuarioResult = await userManager.CreateAsync(usuario, senha);

        if (!usuarioResult.Succeeded)
            return Result.Fail("Erro ao criar usuário.");

        await userManager.AddToRoleAsync(usuario, tipo.ToString());

        return Result.Ok();
    }

    public async Task<Result> LoginAsync(string email, string senha)
    {
        var resultadoLogin = await signInManager.PasswordSignInAsync(
            email,
            senha,
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (resultadoLogin.Succeeded)
            return Result.Ok();

        if (resultadoLogin.IsLockedOut)
            return Result.Fail(ResultadosErro
                .RequisicaoInvalidaErro("Sua conta foi bloqueada temporariamente devido a muitas tentativas inválidas."));

        if (resultadoLogin.IsNotAllowed)
            return Result.Fail(ResultadosErro
                .RequisicaoInvalidaErro("Não é permitido efetuar login. Verifique se sua conta está confirmada."));

        if (resultadoLogin.RequiresTwoFactor)
            return Result.Fail(ResultadosErro
                .RequisicaoInvalidaErro("É necessário confirmar o login com autenticação de dois fatores."));

        return Result.Fail(ResultadosErro
            .RequisicaoInvalidaErro("Login ou senha incorretos."));
    }

    public async Task<Result> LogoutAsync()
    {
        await signInManager.SignOutAsync();

        return Result.Ok();
    }
}
