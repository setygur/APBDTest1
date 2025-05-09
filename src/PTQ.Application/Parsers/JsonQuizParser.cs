using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;
using PTQ.Models.DTOs;

namespace PTQ.Application.Parsers;

public class JsonQuizParser
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<CreateTestDTO?> ParseAsync(Stream input)
    {
        using var reader = new StreamReader(input);
        var jsonText = await reader.ReadToEndAsync();
        var json = JsonNode.Parse(jsonText);
        if (json == null) return null;

        CreateTestDTO? createTestDto = JsonSerializer.Deserialize<CreateTestDTO>(json.ToJsonString(), _options);
        
        if (createTestDto == null)
            return null;

        return createTestDto;
    }
}