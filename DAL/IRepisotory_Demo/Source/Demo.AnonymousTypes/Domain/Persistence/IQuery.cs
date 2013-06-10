namespace Demo.AnonymousTypes.Domain.Persistence
{
	public interface IQuery<TResult>
	{
		TResult Execute();
	}
}