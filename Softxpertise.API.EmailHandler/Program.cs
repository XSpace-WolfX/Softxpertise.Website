using Softxpertise.API.EmailHandler.Services;

var builder = WebApplication.CreateBuilder(args);

// Add secrets configuration during development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add CORS to allow requests from Blazor WebAssembly
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register EmailService with secret configuration
builder.Services.AddSingleton(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var graphApiSection = configuration.GetSection("GraphApi");
    var tenantId = graphApiSection["TenantId"];
    var clientId = graphApiSection["ClientId"];
    var clientSecret = graphApiSection["ClientSecret"];
    return new EmailService(tenantId, clientId, clientSecret, "contact@softxpertise.com");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configurate CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API is running!");

app.Run();
