using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace UsersAPI
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

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("MyUsers"));

            services.AddMvc();

            // this was added to have the message service class intantiated once and retreived on sending message to RabbitMQ Queue.
            services.AddSingleton<IMessageService, MessageService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "UsersAPI", Version = "v1" });
                
                var filePath = Path.Combine(AppContext.BaseDirectory, "UsersAPI.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => { 
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "UsersAPI V1"); 
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // this.AddTestData(app);
        }

        //private  void AddTestData(IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.CreateScope())
        //    {
        //        var context = serviceScope.ServiceProvider.GetService<ApiContext>();

        //        var testUser1 = new UserDTO()
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "Clint",
        //            Surname = "Cassar",
        //            BirthDate = DateTime.ParseExact("30/11/1982", "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        Email = "cassar.clint@gmail.com"
        //        };

        //        context.Users.Add(testUser1);

        //        context.SaveChanges();
        //    } 
        //}
    }
}
