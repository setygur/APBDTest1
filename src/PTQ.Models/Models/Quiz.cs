namespace PTQ.Models.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PotatoTeacherId { get; set; }
    public string PathFile { get; set; }

    public Quiz(int Id, string Name, int PotatoTeacherId, string PathFile)
    {
        Id = Id;
        Name = Name;
        PotatoTeacherId = PotatoTeacherId;
        PathFile = PathFile;
    }
}