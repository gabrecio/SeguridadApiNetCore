using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SEG.Repositories;
using SEG.Repositories.DataContext;
using SEG.Services;
using SEG.Services.Helpers;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;

namespace SeguridadApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Seguridad OSSE", Version = "v1" });
            });

        

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<SeguridadEntities>(options =>
             options.UseSqlServer(
                 Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            /*Repositories*/
            services.AddScoped<IAplicacionRepository, AplicacionRepository>();
             services.AddScoped<IListaPermisoRepository, ListaPermisoRepository>();
                services.AddScoped<IMenuRepository,MenuRepository>();
                services.AddScoped<IOperacionesRepository, OperacionesRepository>();
                services.AddScoped<IRolRepository, RolRepository>();
                services.AddScoped<IUsuarioRepository, UsuarioRepository>();
                services.AddScoped<IUsuarioRolRepository, UsuarioRolRepository>();
                services.AddScoped<ICentroCostoRepository, CentroCostoRepository>();

            //ver Service lifetimes the net core            
            services.AddScoped<IAplicacionService, AplicacionService>();
            services.AddScoped<ICentroCostoService, CentroCostoService>();
            services.AddScoped<IListaPermisoService, ListaPermisoService>();
            services.AddScoped<IPresupuestoService, PresupuestoService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ITangoSueldoService, TangoSueldoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddApplicationInsightsTelemetry();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Core API V1");
            });



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
