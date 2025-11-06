using System.Text;
// REMOVIDO: using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
// REMOVIDO: using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotoMonitoramento.Data;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. CONFIGURAÇÃO DE SERVIÇOS (builder.Services.Add...)
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

    // Sua configuração de Documentação da API (mantendo os valores customizados)
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["Swagger:Title"] ?? "Título Padrão da API", // Adicionado fallback
        Description = (builder.Configuration["Swagger:Description"] ?? "Descrição da API") + DateTime.Now.Year,
        Contact = new OpenApiContact()
        {
            Email = "rm558710@fiap.com.br",
            Name = "Diego Bassalo"
        }
    });

    // 🔹 REMOVIDA toda a configuração JWT (AddSecurityDefinition e AddSecurityRequirement)
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

// REMOVIDA: JWT Authentication section
// REMOVIDO: var jwtKey = builder.Configuration["Jwt:Key"];
// REMOVIDO: var jwtIssuer = builder.Configuration["Jwt:Issuer"];
// REMOVIDO: builder.Services.AddAuthentication(...)

// =========================================================
// 2. CONSTRUÇÃO DA APLICAÇÃO
// =========================================================
var app = builder.Build();

// =========================================================
// 3. CONFIGURAÇÃO DO PIPELINE/MIDDLEWARE (app.Use...)
// =========================================================

// Configuração do Swagger/OpenAPI no pipeline (middleware)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sua API v1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
}

// Aplica a política de CORS
app.UseCors("AllowAll");

// REMOVIDO: app.UseAuthentication();
app.UseAuthorization(); // Mantido para o caso de ter Controllers com [Authorize] que você queira remover depois.

// Mapeamento de endpoints
app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/", () => "✅ API MottuVisualizer funcionando!");


// Configuração de Porta e Execução
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
app.Urls.Add($"http://*:{port}");

app.Run();