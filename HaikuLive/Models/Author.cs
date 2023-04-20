namespace HaikuLive.Models;

public class Author
{
  public Author(int id, string name, DateTime createdAt)
  {
    Id = id;
    Name = name;
    CreatedAt = createdAt;
  }
  public int Id { get; set; }
  public string Name { get; set; }
  public DateTime CreatedAt { get; set; }
  public List<Haiku>? Haikus { get; set; }
}
