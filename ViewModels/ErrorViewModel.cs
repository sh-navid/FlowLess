namespace NoFlowEngine.Models
{
    /// <summary>
    /// Represents a view model for displaying error information.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request ID associated with the error.
        /// </summary>
        /// <remarks>
        /// This can be useful for tracing errors back to specific requests in a system.
        /// </remarks>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be displayed.
        /// </summary>
        /// <remarks>
        /// The request ID is displayed only if it is not null or empty.
        /// </remarks>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}