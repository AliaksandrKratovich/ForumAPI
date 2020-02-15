using AutoMapper;
using Forum.Dal;
using Forum.Dal.DatabaseAccess;
using Forum.Dal.Repositories.UserRepository;
using Forum.Dal.Repository;
using Forum.Models.ArticlesManagement;
using Forum.Models.CommentsManagement;
using Forum.Models.Security;
using Forum.Services.ArticlesManagement;
using Forum.Services.CommentsManagement;
using Forum.Services.UserManagement;
using Forum.WebApi.ErrorHandling;
using Forum.WebApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


namespace Forum.WebApi
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
            services.AddAutoMapper(typeof(ArticleManagementMappingProfile), typeof(CommentManagementMappingProfile));

            services.Configure<Settings>(options =>
            {
                options.ConnectionString
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = Configuration.GetSection("MongoConnection:Database").Value;
                options.CommentsCollection
                    = Configuration.GetSection("MongoConnection:CommentsCollection").Value;
                options.ArticleCollection
                    = Configuration.GetSection("MongoConnection:ArticleCollection").Value;
                options.UserCollection
                    = Configuration.GetSection("MongoConnection:UserCollection").Value;
            });
            services.AddSingleton<ApplicationContext>();

            services.AddScoped<IRepository<Article>, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            var appSettingsSection = Configuration.GetSection("JWTSettings");
            services.Configure<JwtOptions>(appSettingsSection);

            var jwtOptions = appSettingsSection.Get<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Forum WebAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });

            });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Forum WebAPI", Version = "v1" });
            //    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
            //    {
            //        Name = "Bearer",
            //        BearerFormat = "JWT",
            //        Scheme = "bearer",
            //        Description = "Specify the authorization token.",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.Http,
            //    };
            //    c.AddSecurityDefinition("jwt_auth", securityDefinition);

            //    // Make sure swagger UI requires a Bearer token specified
            //    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
            //    {
            //        Reference = new OpenApiReference()
            //        {
            //            Id = "jwt_auth",
            //            Type = ReferenceType.SecurityScheme
            //        }
            //    };
            //    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
            //    {
            //        {securityScheme, new string[] { }},
            //    };
            //    c.AddSecurityRequirement(securityRequirements);
            //});

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndPoint, swaggerOptions.Description);
            });

            app.UseRouting();

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
