using System.Text;

namespace CerbiSharp.Infrastructure.Base.SqlExecuter
{
    public class ConnectionStringBuilder
    {
        public static string Create(ConnectionString cs)
        {

            return new StringBuilder()
                .AppendJoin(';',
                    $"Server={cs.Server}",
                    $"DataBase={cs.DataBase}",
                    $"User Id={cs.UserId}",
                    $"Password={cs.Password}",
                    $"TrustServerCertificate={cs.TrustServerCertificate}"
                    )
                .ToString();
        }
    }
}
