namespace HaikuLive.Models;

public class Haiku
{
  public Haiku
  (
    int id, 
    string line1, 
    string line2, 
    string line3, 
    int liked, 
    string authorName, 
    DateTime createdAt
  )
  {
    Id = id;
    Line1 = line1;
    Line2 = line2;
    Line3 = line3;
    Liked = liked;
    AuthorName = authorName;
    CreatedAt = createdAt;
  }
  public int Id { get; set; }
  public string Line1 { get; set; }
  public string Line2 { get; set; }
  public string Line3 { get; set; }
  public int Liked { get; set; }
  public string AuthorName { get; set; }
  public DateTime CreatedAt { get; set; }
  public int AuthorId { get; set; }
  public Author? Author { get; set; }
  public int TopicId { get; set; }
  public Topic? Topic { get; set; }
}
