namespace Isitar.DependencyUpdater.Api
{
    using System.Text.Json;
    using Application;
    using Application.Common.Services;
    using Common;
    using Git;
    using GitLab;
    using Jobs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Middlewares;
    using NugetUpdater;
    using Persistence;
    using Process;
    using Quartz;
    using Services;

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
            services.AddControllers()
                    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" }); });

            services.AddApplication(Configuration);
            services.AddProcess();
            services.AddPersistence(Configuration);

            services.AddGit(Configuration);
            services.AddNugetUpdater(Configuration);
            services.AddGitlab();

            services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                q.ScheduleJob<MarkingJob>(trigger => trigger
                                                     .WithIdentity("mark projects")
                                                     .WithCronSchedule("0 * * * * ?")
                );

                q.ScheduleJob<UpdateJob>(trigger => trigger
                                                    .WithIdentity("update projects")
                                                    .WithCronSchedule("0 * * * * ?")
                );
            });


            services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });


            services.AddHttpClient();
            services.AddAutoMapper(typeof(ApiMappingProfile).Assembly);
            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));

                app.UseHttpsRedirection();
            }

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            app.UseCors();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}