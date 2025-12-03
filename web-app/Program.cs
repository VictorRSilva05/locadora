using FluentValidation;
using Locadora.Aplicacao.ModuloAluguel;
using Locadora.Aplicacao.ModuloAutenticacao;
using Locadora.Aplicacao.ModuloCliente;
using Locadora.Aplicacao.ModuloCobranca;
using Locadora.Aplicacao.ModuloCombustivel;
using Locadora.Aplicacao.ModuloCondutor;
using Locadora.Aplicacao.ModuloFuncionario;
using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.Aplicacao.ModuloTaxa;
using Locadora.Aplicacao.ModuloVeiculo;
using Locadora.Dominio.Autenticacao;
using Locadora.Dominio.ModuloAluguel;
using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCobranca;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloTaxa;
using Locadora.Dominio.ModuloVeiculo;
using Locadora.Infraestrutura.Compartilhado;
using Locadora.Infraestrutura.ModuloAluguel;
using Locadora.Infraestrutura.ModuloCliente;
using Locadora.Infraestrutura.ModuloCobranca;
using Locadora.Infraestrutura.ModuloCombustivel;
using Locadora.Infraestrutura.ModuloCondutor;
using Locadora.Infraestrutura.ModuloFuncionario;
using Locadora.Infraestrutura.ModuloGrupoVeiculo;
using Locadora.Infraestrutura.ModuloTaxa;
using Locadora.Infraestrutura.ModuloVeiculo;
using Locadora.WebApp.ActionFilters;
using Locadora.WebApp.DependecyInjection;
using Locadora.WebApp.Identity;
using Locadora.WebApp.Orm;
using Microsoft.AspNetCore.Identity;

namespace Locadora.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<GrupoVeiculoAppService>();
            builder.Services.AddScoped<IRepositorioGrupoVeiculo, RepositorioGrupoVeiculoEmOrm>();
            builder.Services.AddScoped<IValidator<GrupoVeiculo>, CadastrarGrupoVeiculoValidator>();
            builder.Services.AddScoped<CombustivelAppService>();
            builder.Services.AddScoped<IRepositorioCombustivel, RepositorioCombustivelEmOrm>();
            builder.Services.AddScoped<IValidator<Combustivel>, CadastrarCombustivelValidator>();
            builder.Services.AddScoped<FuncionarioAppService>();
            builder.Services.AddScoped<IRepositorioFuncionario, RepositorioFuncionarioEmOrm>();
            
            builder.Services.AddScoped<CondutorAppService>();
            builder.Services.AddScoped<IRepositorioCondutor, RepositorioCondutorEmOrm>();
            builder.Services.AddScoped<IValidator<Condutor>, CadastrarCondutorValidator>();
            builder.Services.AddScoped<VeiculoAppService>();
            builder.Services.AddScoped<IRepositorioVeiculo, RepositorioVeiculoEmOrm>();
            builder.Services.AddScoped<IValidator<Veiculo>, CadastrarVeiculoValidator>();
            builder.Services.AddScoped<ClienteAppService>();
            builder.Services.AddScoped<IRepositorioCliente, RepositorioClienteEmOrm>();
            builder.Services.AddScoped<IValidator<Cliente>, CadastrarClienteValidator>();
            builder.Services.AddScoped<TaxaAppService>();
            builder.Services.AddScoped<IRepositorioTaxa, RepositorioTaxaEmOrm>();
            builder.Services.AddScoped<IValidator<Taxa>, CadastrarTaxaValidator>();
            builder.Services.AddScoped<CobrancaAppService>();
            builder.Services.AddScoped<IRepositorioCobranca, RepositorioCobrancaEmOrm>();
            builder.Services.AddScoped<IValidator<Cobranca>,  CadastrarCobrancaValidator>();
            builder.Services.AddScoped<AluguelAppService>();
            builder.Services.AddScoped<IRepositorioAluguel, RepositorioAluguelEmOrm>();


            builder.Services
                 .AddIdentity<User, Role>(options =>
                 {
                     options.Password.RequiredLength = 6;
                     options.Password.RequireDigit = true;
                     options.Password.RequireUppercase = false;
                     options.Password.RequireLowercase = false;
                     options.Password.RequireNonAlphanumeric = false;
                 })
                 .AddEntityFrameworkStores<AppDbContext>()
                 .AddDefaultTokenProviders()
                 .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Autenticacao/Login";
                options.AccessDeniedPath = "/Autenticacao/Denied";
            });

            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
            builder.Services.AddSerilogConfig(builder.Logging, builder.Configuration);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ValidarModeloAttribute>();
            });

            builder.Services.AddScoped<ITenantProvider, IdentityTenantProvider>();
            builder.Services.AddScoped<AutenticacaoAppService>();



            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await IdentitySeeder.SeedAsync(userManager, roleManager);
            }

            if (app.Environment.IsDevelopment())
            {
                app.ApplyMigrations();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/erro");
            }

            app.UseAntiforgery();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}

