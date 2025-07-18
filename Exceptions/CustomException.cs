namespace NoFlowEngine.Exceptions
{
    /// <summary>
    /// A custom exception class that inherits from the base Exception class.
    /// This allows for the creation of specific exception types within the application.
    /// </summary>
    /// <remarks>
    /// This is just an example and can ve removed later
    /// </remarks>
    public class CustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CustomException(string message) : base(message) { }
    }
}