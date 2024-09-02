namespace ReportsProject.Domain.Interfaces.Models;

public interface IEntityId<T> where T : struct
{
    public T Id { get; set; }
}
