using ChatSessionFlow;
using ChatSessionFlow.models;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using SessionStateFlow.package;
using SessionStateFlow.package.Models;
using ToolManagement;
using ToolManagement.ToolDefinitions;
using ToolManagement.ToolDefinitions.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ChatGPTRepo>();
builder.Services.AddSingleton<ToolDefinitionManager>();
builder.Services.AddSingleton<GmailDataAccess>();
builder.Services.AddSingleton<GmailConnector>();

//tools
builder.Services.AddSingleton<IToolDefinition, KnownInformationSearch>();
builder.Services.AddSingleton<IToolDefinition, NewsSearch>();
//builder.Services.AddSingleton<IToolDefinition, FindEmails>();
builder.Services.AddSingleton<IToolDefinition, SendEmail>();
builder.Services.AddSingleton<IToolDefinition, ImageEvaluate>();


//project specific redux stuff
builder.Services.AddScoped<IFlowStateEffects, ChatSessionEffects>();
builder.Services.AddScoped<IFlowStateEffects, ToolUseEffects>();
builder.Services.AddScoped<IFlowStateReducer<ChatSessionEntity>, ChatSessionReducer>();


//general redux stuff
builder.Services.AddScoped<FlowActionHandler>();
builder.Services.AddScoped<FlowState>();
builder.Services.AddScoped<FlowStateData<ChatSessionEntity>>();
builder.Services.AddScoped<IFlowStateDataCore>(sp => sp.GetService<FlowStateData<ChatSessionEntity>>());


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
