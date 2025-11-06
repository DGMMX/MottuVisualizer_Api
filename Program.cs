using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MotoMonitoramento.Data;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. CONFIGURAÇÃO DE SERVIÇOS (builder.Services.Add...)
//    Tudo aqui deve vir ANTES de builder.Build()
// =========================================================

// Banco de dados Oracle
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options
        .UseOracle(builder.Configuration.GetConnectionString("OracleConnection"))
        .EnableSensitiveDataLogging()
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        // Forçamos o TÍTULO PRINCIPAL com fallback.
        Title = builder.Configuration["Swagger:Title"] ?? "MottuVisualizer API",

        Description = (builder.Configuration["Swagger:Description"] ?? "Documentação da API") + DateTime.Now.Year,

        Contact = new OpenApiContact()
        {
            Email = "rm558710@fiap.com.br",
            Name = "Diego Bassalo"
        }
    });
});

// Health Checks
builder.Services.AddHealthChecks();

// Versionamento de API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// =========================================================
// 2. CONSTRUÇÃO DA APLICAÇÃO
// =========================================================
var app = builder.Build();

// =========================================================
// 3. CONFIGURAÇÃO DO PIPELINE/MIDDLEWARE (app.Use...)
//    Tudo aqui deve vir DEPOIS de app.Build()
// =========================================================

// Configuração do Swagger/OpenAPI no pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // 🛑 CORREÇÃO FINAL: Força o nome no dropdown.
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MottuVisualizer V1");

        // CORREÇÃO EXTRA: Força o título da aba do navegador para ajudar contra cache.
        c.DocumentTitle = "MottuVisualizer - Documentação da API";
    });
}
else
{
    app.UseExceptionHandler("/Error");
}

// Aplica a política de CORS
app.UseCors("AllowAll");

app.UseAuthorization();

// Mapeamento de endpoints
app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/", () => "✅ API MottuVisualizer funcionando!");


// Configuração de Porta e Execução
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
app.Urls.Add($"http://*:{port}");

app.Run();