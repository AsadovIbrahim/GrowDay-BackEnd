using System.Net.Mail;

namespace GrowDay.Application.Services
{
    public interface IEmailService
    {


        public bool isHtml { get; set; }
        public MailAddress to { get; set; }
        public MailMessage email { get; set; }
        public MailAddress from { get; set; }


        public Task<bool> sendMailAsync(string mail, string title, string text, bool ishtml);
    }

}
