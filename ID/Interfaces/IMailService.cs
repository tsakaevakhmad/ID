namespace ID.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="subject">Subject of your message</param>
        /// <param name="message">Body message</param>
        /// <returns></returns>
        Task SentAsync(string email, string subject, string message);
    }
}
