using Dapper;
using System;
using System.Data;

namespace Repository.Dapper
{
    public interface IStoredProcedureSaveRepository
    {
        void Save<T>(
            string storedProcedure,
            T entity);
    }

    public class StoredProcedureSaveRepository : IStoredProcedureSaveRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;
        public StoredProcedureSaveRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public void Save<T>(string storedProcedure, T entity)
        {
            var storedProcedureParameters = BuildStoredProcedureParameters(entity);
            ExecuteStoredProcedure(
                storedProcedure: storedProcedure,
                dynamicParameters: storedProcedureParameters);
        }

        private static DynamicParameters BuildStoredProcedureParameters<T>(T entity)
        {
            var result = new DynamicParameters();

            var fieldName = String.Empty;

            if (entity != null)
            {
                var fields = entity.GetType().GetProperties();
                foreach (var field in fields)
                {
                    fieldName = BuildFieldName(field.Name);
                    result.Add(fieldName, field.GetValue(entity, null));
                }
            }

            return result;
        }

        private static string BuildFieldName(string propertyName)
        {
            return String.Format("@{0}", propertyName);
        }

        private void ExecuteStoredProcedure(
            string storedProcedure,
            DynamicParameters dynamicParameters)
        {
            using (var connection = sqlConnectionFactory.Create())
            {
                connection.Open();
                var result = connection
                    .Execute(
                        sql: storedProcedure,
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);
            }
        }
    }
}
