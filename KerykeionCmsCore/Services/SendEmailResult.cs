namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Represents the result of a KerykeionCms send email operation.
    /// </summary>
    public class SendEmailResult
    {
        /// <summary>
        /// Returns an KerykeionCms.Services.SendEmailResult indicating that the email sender is not configured.
        /// </summary>
        public static SendEmailResult IsNotConfigured => new SendEmailResult(false, "Er is nog geen e-mail verzending dienst geconfigureerd.");

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>
        /// True if the operation succeeded, otherwise false.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// An explanatory message to be sent about what exactly caused the operation to fail or succeed.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the SendEmailResult().
        /// </summary>
        /// <param name="success">Flag indicating whether if the operation succeeded or not.</param>
        /// <param name="message">An explanatory message to be sent about what exactly caused the operation to fail or succeed. Optional parameter, defaults to null</param>
        public SendEmailResult(bool success, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
