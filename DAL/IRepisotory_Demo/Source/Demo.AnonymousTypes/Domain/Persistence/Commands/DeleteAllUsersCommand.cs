using System.Configuration;

namespace Demo.AnonymousTypes.Domain.Persistence.Commands
{
	public class DeleteAllUsersCommand : CommandQueryBase, ICommand
	{
        public DeleteAllUsersCommand(string connectionString) : base(connectionString) { }

		public void Execute()
		{
			Execute("DELETE FROM [Users]");
		}
	}
}