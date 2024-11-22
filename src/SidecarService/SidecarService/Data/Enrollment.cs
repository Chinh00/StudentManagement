namespace SidecarService.Data;

public class Enrollment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime StartDate { get; set; }
    public bool IsEnable { get; set; }
    public DateTime EndDate { get; set; }
    public int CourseId { get; set; }
    public string DocumentId { get; set; }
    public int MaxStudent { get; set; }
    
    public ICollection<int> Students { get; set; } = new List<int>();
}