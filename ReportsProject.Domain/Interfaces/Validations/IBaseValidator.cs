using ReportsProject.Domain.Results;


namespace ReportsProject.Domain.Interfaces.Validations;

public interface IBaseValidator<in T> where T : class
{
	public BaseResult ValidateOnNull(T model);

}
