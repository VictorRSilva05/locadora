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
        {
            var erros = usuarioResult.Errors.Select(err =>
            {
                return err.Code switch
                {
                    "DuplicateUserName" => "Já existe um usuário com esse nome.",
                    "DuplicateEmail" => "Já existe um usuário com esse e-mail.",
                    "PasswordTooShort" => "A senha é muito curta.",
                    "PasswordRequiresNonAlphanumeric" => "A senha deve conter pelo menos um caractere especial.",
                    "PasswordRequiresDigit" => "A senha deve conter pelo menos um número.",
                    "PasswordRequiresUpper" => "A senha deve conter pelo menos uma letra maiúscula.",
                    "PasswordRequiresLower" => "A senha deve conter pelo menos uma letra minúscula.",
                    _ => err.Description
                };
            }).ToList();

            return Result.Fail(ResultadosErro.RequisicaoInvalidaErro(erros));
        }

        var tipoString = tipo.ToString();

        var resultadoBuscaCargo = await roleManager.FindByNameAsync(tipoString);

        if (resultadoBuscaCargo is null)
        {
            var cargo = new Role
            {
                Name = tipoString,
                NormalizedName = tipoString.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await roleManager.CreateAsync(cargo);
        }

        await userManager.AddToRoleAsync(usuario, tipoString);

        return await LoginAsync(
            usuario.Email ?? string.Empty,
            senha
        );
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
