using API_CHATBOT_PDF.Handlers;
using API_CHATBOT_PDF.Interfaces;
using API_CHATBOT_PDF.Models;
using API_CHATBOT_PDF.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IFileManagerService, FileManagerService>(); 


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurations
builder.Services.Configure<Configuration>(builder.Configuration.GetSection("Configurations"));

var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
