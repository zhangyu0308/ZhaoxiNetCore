using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCoreWebApi.Utility;

namespace NetCoreWebApi
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
            services.AddScoped<IJWTService, JWTService>();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("V1", new OpenApiInfo()
                {
                    Title = "test",
                    Version = "version-01",
                    Description = "朝夕教育Api学习"
                });
            });


            #region jwt鉴权授权
            var audience = this.Configuration["audience"];
            var issuer = this.Configuration["issuer"];
            var SecurityKey = this.Configuration["SecurityKey"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//默认授权机制
                .AddJwtBearer(options=> {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证
                        ValidateAudience=true,
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        ValidAudience= audience,
                        ValidIssuer= issuer,
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
                    };
                });

            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(s => {
                s.SwaggerEndpoint("/swagger/V1/swagger.json", "test1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            //通过中间件来进行鉴权授权
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
