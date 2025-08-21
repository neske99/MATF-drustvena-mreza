namespace Common.Logger
{
    public interface IFileLogger
    {
        void LogException(string method, string path, Exception exception, long elapsedMs);
    }

    public class FileLogger : IFileLogger
    {
        private readonly string _logDirectory;
        private readonly object _lock = new object();

        public FileLogger(string logDirectory = "logs")
        {
            _logDirectory = ResolvePath(logDirectory);
            EnsureLogDirectoryExists();
        }

        public void LogException(string method, string path, Exception exception, long elapsedMs)
        {
           
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var logLine = $"[{timestamp}] [!] {method} {path} - {exception.Message} ({elapsedMs}ms)";
                
                if (exception.StackTrace != null)
                {
                    logLine += Environment.NewLine + exception.StackTrace;
                }
                
                var fileName = $"errors_{DateTime.Now:yyyy-MM-dd}.log";
                var filePath = Path.Combine(_logDirectory, fileName);

                lock (_lock)
                {
                    File.AppendAllText(filePath, logLine + Environment.NewLine + "---" + Environment.NewLine);
                }
            }

        private string ResolvePath(string logDirectory)
        {
            // If already absolute path, use as is
            if (Path.IsPathRooted(logDirectory))
            {
                return logDirectory;
            }


            return Path.Combine(AppContext.BaseDirectory, logDirectory);
        }

        private void EnsureLogDirectoryExists()
        {
      
                if (!Directory.Exists(_logDirectory))
                {
                    Directory.CreateDirectory(_logDirectory);
                }
    
        }
    }
}