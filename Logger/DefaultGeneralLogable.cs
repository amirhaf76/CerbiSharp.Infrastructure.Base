namespace CerbiSharp.Infrastructure.Base.Logger
{
    public class DefaultGeneralLogable : IGeneralLogable
    {
        private readonly IGeneralLogger _logger;

        public DefaultGeneralLogable()
        {
        }

        public DefaultGeneralLogable(IGeneralLogger logger)
        {
            _logger = logger;
        }

        public bool CanMakeLog
        {
            get
            {
                return Logger != null;
            }
        }

        public IGeneralLogger Logger
        {
            get
            {
                return _logger;
            }
        }

        public bool TryWriteLine(string input)
        {
            var canMakeLog = CanMakeLog;

            if (canMakeLog)
            {
                _logger.WriteLine($"[AppTestlogger]{input}");
            }

            return canMakeLog;
        }
    }
}
