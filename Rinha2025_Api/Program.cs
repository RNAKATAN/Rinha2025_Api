using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.OpenApi.Models;
using Rinha2025_Api.Contratos;
using Rinha2025_Api.DependencyInjection;
using Rinha2025_Api.Domain;
using Rinha2025_Api.UseCases;
using System.Text.Json.Serialization;

namespace Rinha2025_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            // Configure o contexto de source generation
            builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            });



            builder.Services.AddServices();

            // Adicione o serviço Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rinha 2025 api",
                    Version = "v1",
                    Description = "API para testa rinha",
                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = "your.email@example.com",
                        Url = new Uri("https://yourwebsite.com")
                    }
                });
            } );

            builder.Services.Configure<RouteOptions>(options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));


            var app = builder.Build();

            // Configure o middleware Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sua API V1"); // Nome e local do endpoint Swagger
                });
            }

            using (var scopeGet = app.Services.CreateScope())
            {
                var consultaPagamentosUseCase = scopeGet.ServiceProvider.GetRequiredService<IConsultaPagamentosUseCase>();

                var apiGet = app.MapGroup("/");
                apiGet.MapGet("/payments-summary", async (string from, string to) =>
                {
                    await consultaPagamentosUseCase.ConsultarPagamentosPeriodo(from, to);
                    return Results.Ok();
                }
                );

            }

            using (var scopePost = app.Services.CreateScope())
            {
                var orquestradorPagamentos = scopePost.ServiceProvider.GetRequiredService<IOrquestradorPagamentos>();


                var api = app.MapGroup("/");
                api.MapPost("/payments", async (PaymentInput paymentInput) =>
                {
                    await orquestradorPagamentos.Processa(paymentInput);
                    return Results.Ok();
                }
                );
            }







            //var sampleTodos = new Todo[] {
            //    new(1, "Walk the dog"),
            //    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
            //    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
            //    new(4, "Clean the bathroom"),
            //    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
            //};

            //var todosApi = app.MapGroup("/todos");
            //todosApi.MapGet("/", () => sampleTodos);
            //todosApi.MapGet("/{id}", (int id) =>
            //    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
            //        ? Results.Ok(todo)
            //        : Results.NotFound());

            app.Run();
        }
    }

}
