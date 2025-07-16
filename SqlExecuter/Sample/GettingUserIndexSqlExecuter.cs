using System.Data;
using Microsoft.Data.SqlClient;

namespace CerbiSharp.Infrastructure.BaseInfrastructure.SqlExecuter.Sample
{
    internal class GettingUserIndexSqlExecuter : SqlQueryExecuter<UserIndexQueryResult>
    {
        internal string UserId { get; }

        internal GettingUserIndexSqlExecuter(string userId)
        {
            UserId = userId;
        }

        protected override string CreateConnectionString()
        {
            return ConnectionStringBuilder.Create(new ConnectionString
            {
                Server = "192.168.11.53",
                DataBase = "Blockchain",
                UserId = "sa",
                Password = "aA123456",
            });
        }

        protected override string CreateQueryString()
        {
            return
                @"SELECT [Index]
                  FROM [dbo].[UserBlockchainIndex]
                  WHERE [UserId] = @tUserId ";
        }

        protected override SqlCommand ConfigSqlCommand(SqlCommand command)
        {
            base.ConfigSqlCommand(command);

            command.Parameters.AddWithValue("@tUserId", UserId);

            return command;
        }


        protected override UserIndexQueryResult GetData(IDataRecord dataRecord)
        {
            return new UserIndexQueryResult
            {
                Index = (int)dataRecord["Index"],
            };
        }
    }
}


