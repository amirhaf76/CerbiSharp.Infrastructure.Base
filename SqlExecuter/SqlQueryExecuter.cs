using System.Data;
using Microsoft.Data.SqlClient;

namespace CerbiSharp.Infrastructure.BaseInfrastructure.SqlExecuter
{
    public abstract class SqlQueryExecuter<T>
    {
        public IEnumerable<T> Execute()
        {
            var data = new List<T>();
            using (var connection = new SqlConnection(CreateConnectionString()))
            {
                var command = new SqlCommand(CreateQueryString(), connection);

                command = ConfigSqlCommand(command);

                connection.Open();

                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        data.Add(GetData(reader));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return data;
        }


        protected virtual SqlCommand ConfigSqlCommand(SqlCommand command)
        {
            return command;
        }

        protected abstract T GetData(IDataRecord dataRecord);

        protected abstract string CreateConnectionString();

        protected abstract string CreateQueryString();
    }
}