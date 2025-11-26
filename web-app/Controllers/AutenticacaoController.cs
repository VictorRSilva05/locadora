using Locadora.Aplicacao.ModuloAutenticacao;
using Locadora.Dominio.Autenticacao;
using Locadora.WebApp.Extensions;
using Locadora.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace Locadora.WebApp.Controllers;

    [Route("autenticacao")]
    public class AutenticacaoController : Controller
    {
        private readonly AutenticacaoAppService autenticacaoAppService;

        public AutenticacaoController(AutenticacaoAppService autenticacaoAppService)
        {
            this.autenticacaoAppService = autenticacaoAppService;
        }

        [HttpGet("registro")]
        public IActionResult Registro()
        {
            if (User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Index", "Home");

            var registroVm = new RegistroViewModel();

            return View(registroVm);
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroViewModel registroVm)
        {
            var usuario = new User()
            {
                UserName = registroVm.Email,
                Email = registroVm.Email,
            };

            var resultado = await autenticacaoAppService.RegistrarAsync(
                usuario,
                registroVm.Senha ?? string.Empty,
                registroVm.Tipo
            );

            if (resultado.IsFailed)
                return this.PreencherErrosModelState(resultado, registroVm);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            var loginVm = new LoginViewModel();

            ViewData["ReturnUrl"] = returnUrl;

            return View(loginVm);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginVm, string? returnUrl = null)
        {
            var resultado = await autenticacaoAppService.LoginAsync(
                loginVm.Email ?? string.Empty,
                loginVm.Senha ?? string.Empty
            );

            if (resultado.IsFailed)
                return this.PreencherErrosModelState(resultado, loginVm);

            if (Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await autenticacaoAppService.LogoutAsync();

            return RedirectToAction(nameof(Login));
        }
    }