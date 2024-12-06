var builder = WebApplication.CreateBuilder(args);

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
        policy.WithOrigins("https://localhost:5001") // URL de ton Blazor WebAssembly
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Ajouter les services nécessaires pour l'API
builder.Services.AddControllers();

var app = builder.Build();

// Utiliser CORS dans le pipeline
app.UseCors("AllowBlazor");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
