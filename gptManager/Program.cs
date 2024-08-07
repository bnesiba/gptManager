using ActionFlow;
using ChatSessionFlow;
using ChatSessionFlow.models;
using GoogleCloudConnector.GmailAccess;
using OpenAIConnector.ChatGPTRepository;
using ToolManagement;
using ToolManagement.ToolDefinitions;
using ToolManagement.ToolDefinitions.Models;
using ToolManagement.ToolDefinitions.StoryEvalTools;

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

builder.Services.AddSingleton<IToolDefinition, SetCharacterList>();
builder.Services.AddSingleton<IToolDefinition, SetStoryTags>();

//redux flow stuff
builder.Services.UseFlowState();
builder.Services.UseEffects<ChatSessionEffects>();
builder.Services.UseEffects<ToolUseEffects>();
builder.Services.UseEffects<StoryEvalEffects>();
builder.Services.UseReducer<ChatSessionReducer, ChatSessionEntity>();


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
