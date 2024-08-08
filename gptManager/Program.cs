using ActionFlow;
using ChatSessionFlow;
using ChatSessionFlow.models;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using StoryEvaluatorFlow;
using StoryEvaluatorFlow.Models;
using ToolManagement;
using ToolManagement.ToolDefinitions;
using ToolManagement.ToolDefinitions.Models;
using ToolManagement.ToolDefinitions.StoryEvaluatorTools;

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

//assistant tools
builder.Services.AddSingleton<IToolDefinition, KnownInformationSearch>();
builder.Services.AddSingleton<IToolDefinition, NewsSearch>();
//builder.Services.AddSingleton<IToolDefinition, FindEmails>();
builder.Services.AddSingleton<IToolDefinition, SendEmail>();
builder.Services.AddSingleton<IToolDefinition, ImageEvaluate>();

//story eval tools
builder.Services.AddSingleton<IToolDefinition, SetCharacterList>();
builder.Services.AddSingleton<IToolDefinition, SetStoryTags>();
builder.Services.AddSingleton<IToolDefinition, SetGeneralInfo>();
builder.Services.AddSingleton<IToolDefinition, SetStorySummary>();

//redux flow stuff
builder.Services.UseFlowState();
builder.Services.UseEffects<ChatSessionEffects>();
builder.Services.UseEffects<ToolUseEffects>();
builder.Services.UseReducer<ChatSessionReducer, ChatSessionEntity>();

//story eval flow stuff
builder.Services.UseEffects<StoryEvaluatorEffects>();
builder.Services.UseReducer<StoryEvaluationReducer, StoryEvaluatorEntity>();


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
