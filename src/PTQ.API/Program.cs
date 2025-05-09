using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using PTQ.Application;
using PTQ.Application.Parsers;
using PTQ.Models.DTOs;
using PTQ.Repositories.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IService, Service>(
    _ => new Service(connectionString));

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/api/quizzes", (IService service) =>
{
    try
    {
        return Results.Ok(service.GetAllQuizzes());
    }
    catch (Exception e)
    {
        return Results.Problem();
    }
});
app.MapGet("/api/quizzes/{id}", (IService service, string id) =>
{
    try
    {
        return Results.Ok(service.GetQuizById(id));
    }
    catch (NotFoundException e)
    {
        return Results.NotFound();
    }
    catch (Exception e)
    {
        return Results.Problem();
    }
});
app.MapPost("/api/quizzes",async (IService service, HttpRequest request) =>
{
    var parser = new JsonQuizParser();
    
    try
    {
        CreateTestDTO createTestDto = await parser.ParseAsync(request.Body);

        if (createTestDto is null)
        {
            return Results.NotFound();
        }

        var result = service.AddQuiz(createTestDto);
        if (result is true)
        {
            return Results.NoContent();
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    catch (JsonException e)
    {
        return Results.BadRequest($"Invalid JSON: {e.Message}");
    }
    catch (ArgumentException e)
    {
        return Results.BadRequest($"Invalid Arguments: {e.Message}");
    }
    catch (Exception e)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}).Accepts<string>("application/json");

app.Run();


