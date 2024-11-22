namespace SidecarService.Data.Domain;

public class Course
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int SubjectId { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
}