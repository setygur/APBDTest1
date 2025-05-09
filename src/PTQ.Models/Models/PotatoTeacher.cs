namespace PTQ.Models.Models;

public class PotatoTeacher
{
    public int Id { get; set; }
    public string Name { get; set; }

    public PotatoTeacher(int id, string name)
    {
        Id = id;
        Name = name;
    }
}