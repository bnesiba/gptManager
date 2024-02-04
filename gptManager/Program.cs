using ContextManagement;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using ToolManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ChatGPTRepo, ChatGPTRepo>();
builder.Services.AddSingleton<ChatContextManager, ChatContextManager>();
builder.Services.AddSingleton<ToolDefinitionManager, ToolDefinitionManager>();
builder.Services.AddSingleton<GmailDataAccess, GmailDataAccess>();
builder.Services.AddSingleton<GmailConnector, GmailConnector>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
