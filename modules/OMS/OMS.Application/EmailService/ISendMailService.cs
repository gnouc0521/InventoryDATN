using Abp.Application.Services;
using bbk.netcore.Web.EmailService;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public interface ISendMailService : IApplicationService
{
    Task SendMail(MailContent mailContent);

    Task SendEmailAsync(string email, string subject, string htmlMessage,string path);

    Task SendEmailCvAsync(string email, string subject, string htmlMessage);
}
