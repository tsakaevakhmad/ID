using ID.Interfaces;

namespace ID.Services
{
    public class EmailServiceMock : IMailService
    {
        private readonly ILogger<EmailServiceMock> _logger;

        public EmailServiceMock(ILogger<EmailServiceMock> logger) 
        {
            _logger = logger;
        }

        public async Task SentAsync(string email, string subject, string message)
        {
            try
            {
                await Task.Run(() =>
                {
                    _logger.LogWarning($"Email: {email} \n Subject: {subject} \n Message: {message}");
                });
            }
            catch
            {
                _logger.LogError("Error sending email");

            }
        }   
    }
}
