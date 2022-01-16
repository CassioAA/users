using System.Text;
using AutoMapper;
using Manager.API.ViewModels;
using Manager.API.Token;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Infra.Context;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Domain.Entities;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EscNet.IoC.Cryptography;

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

            #region JWT

            var secretKey = Configuration["Jwt:Key"];

            services.AddAuthentication(x =>{

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            }).AddJwtBearer(x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            #endregion

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
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            #endregion


            #region Swagger

            services.AddSwaggerGen(c =>
            {
                // API description
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Users Manager API",
                    Version = "v1",
                    Description = "API para gerenciamento de usuários.",
                    Contact = new OpenApiContact
                    {
                        Name = "Cassio Almeida",
                        Email = "cassioalmeidaccti@gmail.com",
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor utilize Bearer <TOKEN>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });

            #endregion

            services.AddRijndaelCryptography(Configuration["Cryptography"]);
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}