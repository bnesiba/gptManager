using ContextManagement;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using SessionStateFlow;
using SessionStateFlow.package;
using SessionStateFlow.package.Models;
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

//redux stuff
builder.Services.AddScoped<FlowState, FlowState>();
builder.Services.AddScoped<IFlowStateReducer<ProjectStateModel>, ProjectReducer>();
builder.Services.AddScoped<FlowStateData<ProjectStateModel>, FlowStateData<ProjectStateModel>>();


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
