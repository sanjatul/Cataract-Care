using Microsoft.AspNetCore.Mvc;

namespace Cataract_Care.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string message);
    }
}
