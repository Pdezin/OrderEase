namespace Domain.Workflows.ErrorTracking
{
    public class Error
    {
        public string Property { get; private set; }
        public string Message { get; private set; }
        public object? Value { get; private set; }

        public Error(string property, string message, object? value = null)
        {
            Property = property;
            Message = message;
            Value = value;
        }
    }
}
