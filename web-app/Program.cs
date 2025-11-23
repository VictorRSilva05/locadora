using Locadora.Aplicacao.ModuloCliente;
using Locadora.Aplicacao.ModuloCombustivel;
using Locadora.Aplicacao.ModuloCondutor;
using Locadora.Aplicacao.ModuloFuncionario;
using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.Aplicacao.ModuloVeiculo;
using Locadora.Dominio.ModuloCliente;
using Locadora.Dominio.ModuloCombustivel;
using Locadora.Dominio.ModuloCondutor;
using Locadora.Dominio.ModuloFuncionario;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloVeiculo;
using Locadora.Infraestrutura.ModuloCliente;
using Locadora.Infraestrutura.ModuloCombustivel;
using Locadora.Infraestrutura.ModuloCondutor;
using Locadora.Infraestrutura.ModuloFuncionario;
using Locadora.Infraestrutura.ModuloGrupoVeiculo;
using Locadora.Infraestrutura.ModuloVeiculo;
using Locadora.WebApp.ActionFilters;
using Locadora.WebApp.DependecyInjection;
using Locadora.WebApp.Orm;

namespace Locadora.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddScoped<GrupoVeiculoAppService>();
            builder.Services.AddScoped<IRepositorioGrupoVeiculo, RepositorioGrupoVeiculoEmOrm>();
            builder.Services.AddScoped<CombustivelAppService>();
            builder.Services.AddScoped<IRepositorioCombustivel, RepositorioCombustivelEmOrm>();
            builder.Services.AddScoped<FuncionarioAppService>();
            builder.Services.AddScoped<IRepositorioFuncionario, RepositorioFuncionarioEmOrm>();
            builder.Services.AddScoped<CondutorAppService>();
            builder.Services.AddScoped<IRepositorioCondutor, RepositorioCondutorEmOrm>();
            builder.Services.AddScoped<VeiculoAppService>();
            builder.Services.AddScoped<IRepositorioVeiculo, RepositorioVeiculoEmOrm>();
            builder.Services.AddScoped<ClienteAppService>();
            builder.Services.AddScoped<IRepositorioCliente, RepositorioClienteEmOrm>();

            builder.Services.AddEntityFrameworkConfig(builder.Configuration);
            builder.Services.AddSerilogConfig(builder.Logging, builder.Configuration);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ValidarModeloAttribute>();
            });

            var app = builder.Build();

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
