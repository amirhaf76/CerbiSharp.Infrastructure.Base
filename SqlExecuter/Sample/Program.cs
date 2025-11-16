namespace CerbiSharp.Infrastructure.Base.SqlExecuter.Sample
{
    public class SampleProgram
    {
        public static void Execute()
        {
            var queryExecuter1 = new GettingBasicInfoSqlExecuter("btc", "btc").Execute();

            var obj = queryExecuter1.FirstOrDefault() ?? throw new Exception();

            var queryExecuter2 = new GettingTatumIdSqlExecuter(
                obj.CurrncyId,
                obj.MoneyNetworkId,
                3).Execute();

            var queryExecuter3 = new GettingUserIndexSqlExecuter("f1be25d2-623d-4221-1d57-08dac6fb5ae3").Execute();

            Console.WriteLine(queryExecuter2.FirstOrDefault()?.TatumAccountId);
            Console.WriteLine(queryExecuter3.FirstOrDefault()?.Index);
        }
    }
}