using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Repository.Dapper
{
    public interface IStoredProcedureSearchRepository
    {
        IReadOnlyCollection<TResult> Search<TResult>(
            string storedProcedure,
            dynamic parameters = null);
    }
    public class StoredProcedureSearchRepository : IStoredProcedureSearchRepository
    {
        private readonly SqlConnectionFactory sqlConnectionFactory;
        public StoredProcedureSearchRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public IReadOnlyCollection<TResult> Search<TResult>(
            string storedProcedure,
            dynamic parameters = null)
        {
            var storedProcedureParameters = BuildStoredProcedureParameters(parameters);
            var result = ExecuteStoredProcedure<TResult>(
                storedProcedure: storedProcedure,
                dynamicParameters: storedProcedureParameters);
            return result as IReadOnlyCollection<TResult>;
        }

        private static DynamicParameters BuildStoredProcedureParameters(dynamic parameters)
        {
            var result = new DynamicParameters();
            if (parameters == null)
            {
                return result;
            }
            var fieldName = String.Empty;
            var fields = parameters.GetType().GetProperties();
            foreach (var field in fields)
            {
                fieldName = BuildFieldName(field.Name);
                result.Add(fieldName, field.GetValue(parameters, null));
            }
            return result;
        }

        private static string BuildFieldName(string propertyName)
        {
            return String.Format("@{0}", propertyName);
        }

        private IReadOnlyCollection<TResult> ExecuteStoredProcedure<TResult>(
            string storedProcedure,
            DynamicParameters dynamicParameters)
        {
            using (var connection = sqlConnectionFactory.Create())
            {
                connection.Open();
                var result = connection
                    .Query<TResult>(
                        sql: storedProcedure,
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);
                return result
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}
