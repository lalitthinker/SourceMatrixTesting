using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApi.Models.Email;

namespace IdentityApi.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailModel email);
        Task<bool> SendEmailBySendGrid(string SendToEmail, string Subject, string Body, string SendToName = "Customer");
    }
}
