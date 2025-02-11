namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
