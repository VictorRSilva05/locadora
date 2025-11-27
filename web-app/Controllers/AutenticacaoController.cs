using Locadora.Aplicacao.ModuloAutenticacao;
using Locadora.Dominio.Autenticacao;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Policy;

namespace Locadora.WebApp.Controllers;

[Route("Autenticacao")]
public class AutenticacaoController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AutenticacaoController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("Login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        var resultado = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Senha,
            model.LembrarMe,
            lockoutOnFailure: false
        );

        if (!resultado.Succeeded)
        {
            ModelState.AddModelError("", "Email ou senha incorretos.");
            return View(model);
        }

        if (returnUrl != null)
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}