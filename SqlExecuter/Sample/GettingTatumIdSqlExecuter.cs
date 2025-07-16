using System.Data;
using Microsoft.Data.SqlClient;

namespace CerbiSharp.Infrastructure.BaseInfrastructure.SqlExecuter.Sample
{
    internal class GettingTatumIdSqlExecuter : SqlQueryExecuter<TatumQueryResult>
    {
        internal int MoneyNetworkId { get; }

        internal int CurrencyId { get; }

        internal int DomainId { get; }

        internal GettingTatumIdSqlExecuter(int currencyId, int moneyNetworkId, int domainId)
        {
            CurrencyId = currencyId;
            MoneyNetworkId = moneyNetworkId;
            DomainId = domainId;
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
                @"SELECT
	                bl.TatumAccountId AS TatumAccountId
                FROM
	                [Blockchain].[dbo].[BCProvider] AS bcp
	                INNER JOIN [Blockchain].[dbo].[BlockchainWallet] AS bw ON bw.[BCProviderId] = bcp.Id
	                INNER JOIN [Blockchain].[dbo].[BlockchainLedger] AS bl ON bl.BlockchainWalletId = bw.[Id]
                WHERE 
	                bcp.[DomainId] = @pDomainId AND bw.[MoneyNetworkId] = @pMoneyNetworkId AND bl.[CurrencyId] = @pCurrencyId";
        }

        protected override SqlCommand ConfigSqlCommand(SqlCommand command)
        {
            base.ConfigSqlCommand(command);

            command.Parameters.AddWithValue("@pCurrencyId", CurrencyId);
            command.Parameters.AddWithValue("@pMoneyNetworkId", MoneyNetworkId);
            command.Parameters.AddWithValue("@pDomainId", DomainId);

            return command;
        }

        protected override TatumQueryResult GetData(IDataRecord dataRecord)
        {
            return new TatumQueryResult
            {
                TatumAccountId = dataRecord["TatumAccountId"] as string,
            };
        }
    }
}