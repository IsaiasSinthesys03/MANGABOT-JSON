/*
* Configuracion de la API de Mangas. Ahora incluye Swashbuckle (Swagger) para documentacion.
* Asegurate de que la referencia a Swashbuckle.AspNetCore este en MiMangaBot.csproj.
*/
using MiMangaBot.Infrastructure;
using MiMangaBot.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen; // Asegurate de que este using este presente

var builder = WebApplication.CreateBuilder(args);

// Configuracion de servicios para la inyeccion de dependencias (DI).
builder.Services.AddScoped<MangaServices>();
builder.Services.AddTransient<MangaRepository>();

builder.Services.AddControllers();

// Configuracion de Swashbuckle para generar la documentacion OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Agrega los servicios de Swagger

var app = builder.Build();

// Configuracion del pipeline de solicitudes HTTP (middleware).
if (app.Environment.IsDevelopment())
{
    // Habilita el middleware de Swagger para servir el JSON generado.
    app.UseSwagger();
    // Habilita el middleware de SwaggerUI para servir la interfaz interactiva.
    // La UI estara disponible en /swagger
    app.UseSwaggerUI();
}

// REDIRECCION HTTPS COMENTADA (Asegurate que esta linea este con // o eliminada):
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();