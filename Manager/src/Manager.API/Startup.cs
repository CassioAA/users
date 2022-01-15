using System.Xml.Serialization;
using System.Buffers;
using AutoMapper;
using Manager.API.ViewModels;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Infra.Context;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;

namespace Manager.API
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

            services.AddControllers();

            #region AutoMapper

            var AutoMapperConfig = new MapperConfiguration(cfg => {

                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
                cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
            });

            services.AddSingleton(AutoMapperConfig.CreateMapper());

            #endregion

            #region Dependency Injection

            services.AddSingleton(d => Configuration);
            services.AddDbContext<ManagerContext>(options => options
                .UseSqlServer(Configuration["ConnectionStrings:USERSMANAGER"]),
                ServiceLifetime.Transient
            );
            services.AddScoped<IUserService, UserServices>();
            services.AddScoped<IUserRepository, UserRepository>();

            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api_Net6", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // configuration of the middlewares (which are in the HTTP request pipeline)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api_Net6 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}