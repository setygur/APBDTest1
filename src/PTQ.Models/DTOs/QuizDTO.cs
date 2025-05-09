namespace PTQ.Models.DTOs;

public class QuizDTO
{
    public int Id { get; set; }
    public string Name { get; set; }

    public QuizDTO(int id, string name)
    {
        Id = id;
        Name = name;
    }
}