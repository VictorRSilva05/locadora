using Locadora.Aplicacao.ModuloGrupoVeiculo;
using Locadora.Dominio.ModuloGrupoVeiculo;
using Locadora.Infraestrutura.ModuloGrupoVeiculo;
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
