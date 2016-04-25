namespace AzureDiagnostics
{
    public class LogRecord
    {
        public System.DateTimeOffset Time;
        public string ResourceId;
        public string Category;
        public string OperationName;
        public string ResultType;
        public string ResultSignature;
        public string CorrelationId;
        public string Identity;
    }

    public class LogRecord<T> : LogRecord
    {
        public T Properties;
    }
}