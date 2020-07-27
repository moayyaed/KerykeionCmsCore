using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<SendEmailResult> SendEmailAsync(string username, string email, string subject, string message);
    }
}
