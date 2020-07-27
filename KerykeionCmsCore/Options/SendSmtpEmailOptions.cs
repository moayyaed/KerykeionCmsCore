namespace KerykeionCmsCore.Options
{
    /// <summary>
    /// Represents the options to configure the SendEmailService.
    /// </summary>
    public class SendSmtpEmailOptions
    {
        /// <summary>
        /// Gets or sets the name of the application to use as a sender for an email.
        /// </summary>
        public string WebsiteName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the application to use as a sender for an email.
        /// </summary>
        public string WebsiteEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Smtp host name.
        /// </summary>
        public string SmtpHostName { get; set; }

        /// <summary>
        /// Gets or sets the Smtp host port.
        /// </summary>
        public int SmtpHostPort { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating wether to use ssl or not.
        /// </summary>
        public bool SmtpUseSsl { get; set; }

        /// <summary>
        /// Gets or sets the email address to be authenticated for the smtp host.
        /// </summary>
        public string AuthenticatedEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the password for the email address to be authenticated for the smtp host.
        /// </summary>
        public string AuthenticatedEmailAddressPassword { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating wether the email sender is in fact configured. Defaults to False.
        /// </summary>
        /// <value>
        /// True if the email sender is configured, false if it's not.
        /// </value>
        public bool IsConfigured { get; set; } = false;
    }
}