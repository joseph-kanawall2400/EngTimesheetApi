using EngTimesheetApi.Data;
using EngTimesheetApi.Factories;
using EngTimesheetApi.Interfaces;
using EngTimesheetApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace EngTimesheetApi
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
			services.AddMvc();


			services.AddDbContext<TimesheetContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:EngTimesheet"]))
				.AddScoped<ITokenService, TokenService>((provider) => TokenServiceFactory.CreateTokenService(Configuration, provider))
				.AddScoped<IEmailTokenService, EmailTokenService>((provider) => EmailTokenServiceFactory.CreateEmailTokenService(Configuration.GetSection("Email"), provider));

			services.AddSwaggerGen(options => options.SwaggerDoc("v1", new Info
			{
				Title = "Engineering Timesheet API",
				Version = "v1"
			}));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

			app.UseSwagger();
			app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Engineering Timesheet API v1"));
		}
	}
}
