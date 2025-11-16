namespace CerbiSharp.Infrastructure.Base.SqlExecuter
{
    public class ConnectionString
    {
        public ConnectionString()
        {
            Server = string.Empty;
            DataBase = string.Empty;
            UserId = string.Empty;
            Password = string.Empty;
            TrustServerCertificate = true;
        }

        public string Server { get; set; }

        public string DataBase { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public bool TrustServerCertificate { get; set; }
    }
}
