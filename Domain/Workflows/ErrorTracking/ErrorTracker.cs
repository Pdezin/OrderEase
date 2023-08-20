namespace Domain.Workflows.ErrorTracking
{
    public abstract class ErrorTracker
    {
        private readonly List<Error> _errors;

        public ErrorTracker()
        {
            _errors = new List<Error>();
        }

        public List<Error> Errors => _errors;
        public bool IsValid => _errors.Count == 0;
        public bool IsNotFound { get; private set; }

        public void AddErrors(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public void AddError(Error error)
        {
            _errors.Add(error);
        }

        public void AddError(string property, string message, object? value = null)
        {
            _errors.Add(new Error(property, message, value));
        }

        public void NotFound(string property, string message)
        {
            IsNotFound = true;
            _errors.Add(new Error(property, message, null));
        }
    }
}
