namespace SidecarService.Data.Domain;

public class PersonalCourse
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Course Course { get; set; } = null!;
}
