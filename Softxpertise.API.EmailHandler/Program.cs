using Softxpertise.API.EmailHandler.Services;

var builder = WebApplication.CreateBuilder(args);

// Add secrets configuration during development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Ajouter les paramètres de configuration (Azure App Settings ou secrets locaux)
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add CORS to allow requests from Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://softxpertise.com/",
                           "https://softxpertise.com",// Application déployée
                           "http://localhost:5000",
                           "http://localhost:5001",
                           "https://localhost:7181") // Environnement local
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register EmailService with secret configuration
builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    var tenantId = configuration["GraphApi-TenantId"];
    var clientId = configuration["GraphApi-ClientId"];
    var clientSecret = configuration["GraphApi-ClientSecret"];

    if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
    {
        throw new Exception("Les informations de GraphApi sont manquantes dans la configuration !");
    }

    return new EmailService(tenantId, clientId, clientSecret, "contact@softxpertise.com");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configurate CORS
app.UseCors("AllowBlazor");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API is running!");

app.Run();
