using Microsoft.Data.SqlClient;
using System.Data;

namespace CerbiSharp.Infrastructure.Base.SqlExecuter.Sample
{
    internal class GettingBasicInfoSqlExecuter : SqlQueryExecuter<BasicInfoQueryResult>
    {
        internal string CurrencySymbol { get; }
        internal string MoneyNetworkSymbol { get; }

        internal GettingBasicInfoSqlExecuter(string currencySymbol, string moneyNetworkSymbol)
        {
            CurrencySymbol = currencySymbol;
            MoneyNetworkSymbol = moneyNetworkSymbol;
        }

        protected override string CreateConnectionString()
        {
            return ConnectionStringBuilder.Create(new ConnectionString
            {
                Server = "192.168.11.53",
                DataBase = "BaseInfo",
                UserId = "sa",
                Password = "aA123456",
            });
        }

        protected override string CreateQueryString()
        {
            return
                @"SELECT
	                csm.[CurrencyId],
                    csm.[MoneyNetworkId]
                FROM
	                [dbo].[CurrencySupportedMoneyNetwork] AS csm 
	                INNER JOIN [dbo].[Currency] AS c  ON  c.[Id] = csm.[CurrencyId]
	                INNER JOIN [dbo].[MoneyNetwork] AS m ON m.[Id] = csm.[MoneyNetworkId]
                WHERE
	                m.[Enabled] = 1 AND c.[Symbol] = @pCurrencySymbol AND m.[Symbol] = @pMoneyNetworkSymbol";
        }

        protected override BasicInfoQueryResult GetData(IDataRecord dataRecord)
        {
            return new BasicInfoQueryResult
            {
                CurrncyId = (int)dataRecord[0],
                MoneyNetworkId = (int)dataRecord[1],
            };
        }

        protected override SqlCommand ConfigSqlCommand(SqlCommand command)
        {
            base.ConfigSqlCommand(command);

            command.Parameters.AddWithValue("@pCurrencySymbol", CurrencySymbol);
            command.Parameters.AddWithValue("@pMoneyNetworkSymbol", MoneyNetworkSymbol);

            return command;
        }
    }
}
