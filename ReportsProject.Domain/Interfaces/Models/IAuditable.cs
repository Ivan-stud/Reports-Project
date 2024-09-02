namespace ReportsProject.Domain.Interfaces.Models;

public interface IAuditable<T> where T : struct
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public T CreatedBy { get; set; }
    public T? UpdatedBy { get; set; }
}
