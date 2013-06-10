using System.Linq;
using System.Data;
using Demo.AnonymousTypes.Domain.Entities;

namespace Demo.AnonymousTypes.Domain.Persistence.Commands
{
    public class DeleteUserByIdCommand : CommandQueryBase, ICommand
    {
        private int Id;

        public DeleteUserByIdCommand(string connectionString, int id)
            : base(connectionString)
        {
            Id = id;
        }

        public void Execute()
        {
            const string sql = @"DELETE [Users] 
                                WHERE ID = @Id";
            Execute(sql, new { Id });
        }
    }
}
