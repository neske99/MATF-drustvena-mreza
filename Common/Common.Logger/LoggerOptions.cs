namespace Common.Logger
{
    public class LoggerOptions
    {
        public bool LogBodies { get; set; } = true;

        public bool LogHeaders { get; set; } = true;

        public List<string> ExcludedPaths { get; set; } = new List<string>
        {
            "/health",
            "/swagger",
            "/favicon.ico"
        };
    }
}