namespace PTQ.Models.DTOs;

public class CreateTestDTO
{
    public string Name { get; set; }
    public string PotatoTeacherName { get; set; }
    public string Path { get; set; }

    public CreateTestDTO(string name, string potatoTeacherName, string path)
    {
        Name = name;
        PotatoTeacherName = potatoTeacherName;
        Path = path;
    }
}