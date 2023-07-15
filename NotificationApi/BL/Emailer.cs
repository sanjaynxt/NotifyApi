using NotificationApi.Models;
using System.Net.Mail;
using System.Net;

namespace NotificationApi.BL
{
    public class Emailer
    {
        private readonly IConfiguration _configuration;
        public Emailer(IConfiguration config)
        {
            _configuration = config;
        }
        public EmailResponse SendEMail(EmailData ed)
        {
            string subject = "", html_body = "";
            string key = RandomString(32);
            string from = _configuration["UserEmail"].ToString();
            var resp = new EmailResponse();
            resp.key = key;
            if (ed.mailtype == "email-verification")
            {
                subject = "Registration Link - FOBOH";
                html_body = PrepareMessage("1", key);
            }
            else if (ed.mailtype == "password-reset")
            {
                subject = "Reset your Password";
                html_body = PrepareMessage("2", key, ed.name);
            }
            var fromAddress = new MailAddress(from, "FOBOH");
            var toAddress = new MailAddress(ed.To);

            try
            {
                //Send(fromAddress, toAddress, html_body, subject);
                SendViaGmail(fromAddress, toAddress, html_body, subject);
                resp.message = "Email Sent Successfully";
                resp.status = 200;
            }
            catch (Exception ex)
            {
                resp.message = "Email sending failed";
                resp.status = 400;
            }
            return resp;
        }

        private void Send(MailAddress from, MailAddress To, string body, string subject)
        {
            string fromPassword = "Dwarka@123";//Get from Config
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(from, To)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                From = new MailAddress(from.Address, "FOBOH")
            })
            {
                smtp.Send(message);
            }
        }
        private void SendViaGmail(MailAddress from, MailAddress To, string body, string subject)
        {
            string fromPassword = _configuration["UserPassword"].ToString();
            string Host = _configuration["Host"].ToString();
            string PortId = _configuration["Port"].ToString();
            //string fromPassword = "kuxsaiazeuuymcgd";//Get from Config
            var smtp = new SmtpClient
            {
                Host = Host,// "smtp.gmail.com",
                Port = Convert.ToInt32(PortId),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(from, To)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                From = new MailAddress(from.Address, "FOBOH")
            })
            {
                smtp.Send(message);
            }
        }

        private string PrepareMessage(string type, string key = "", string name = "")
        {
            string body = "";
            string url = _configuration["SiteUrl"].ToString();
            if (type == "1")
            {
                body = "<head><meta charset=\"UTF - 8\"><meta name=\"viewport\" content=\"width = device - width,initial - scale = 1\"><link href=\"https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap\" rel=\"stylesheet\"><title>Document</title></head><body><div class=\"email-bg\" style=\"max-width:600px;background:#f8fafc;margin:0 auto;padding:30px 0\"><div class=\"logo\" style=\"text-align:center;padding:10px 0\"><img src=\"https://dev-orderflow.foboh.com.au/static/media/fobohLogo.7e95493b401df09599df.png\" alt=\"\" style=\"max-width:150px\"></div><div class=\"inner-content\" style=\"max-width:500px;margin:10px 0;padding:20px 30px;background:#fff;margin:20px auto\"><h1 style=\"color:#147d73;font-size:32px;font-family:Inter,sans-serif;font-style:normal;font-weight:700;line-height:100%;letter-spacing:-.32px;margin-bottom:25px\">Get started with FOBOH</h1><p style=\"font-family:Inter,sans-serif;font-size:15px;color:#637381;margin-bottom:25px\">Hi there,</p><p style=\"font-family:Inter,sans-serif;font-size:15px;color:#637381;margin-bottom:30px\">Please activate your account by clicking the button below.</p><div style=\"display:flex;justify-content:center\"><a href=\"" + url + "/auth/registration/" + key + "\" style=\"border-radius:8px;border:1px solid #f8fafc;background:#147d73;display:block;padding:6px 20px;text-decoration:none;color:#fff;font-family:Inter,sans-serif;margin:0 auto\">Verify my email</a></div><p style=\"font-size:12px;text-align:center;font-family:Inter,sans-serif;color:#637381;margin-top:20px\">Please note this link expires in 7 days..</p><p style=\"font-size:15px;font-weight:600;color:#637381;font-family:Inter,sans-serif;margin-top:30px;margin-bottom:25px\">Didn’t request this email?<span style=\"font-size:16px;font-weight:400\">Just ignore me..</span></p><p style=\"font-size:15px;font-weight:600;color:#637381;font-family:Inter,sans-serif\">Having trouble?<a href=\"\" style=\"font-size:15px;font-weight:600;color:#147d73\">Let us help you</a></p><p style=\"color:#637381;font-size:15px;font-weight:400;font-family:Inter,sans-serif;line-height:30px;margin-top:25px\">Thanks,<br>Team FOBOH</p></div><div class=\"bottom-footer\" style=\"text-align:center\"><p style=\"color:#637381;font-size:12px;font-weight:400;font-family:Inter,sans-serif;margin-top:60px;text-align:center;line-height:18px\">FOBOH Pty Ltd<br>ABN 47 668 157 739</p><a href=\"\" style=\"color:#637381;font-size:12px;font-weight:600;font-family:Inter,sans-serif;text-align:center;line-height:18px;text-decoration:none\">Terms & conditions</a><span style=\"color:#637381;padding:0 10px\">|</span><a href=\"\" style=\"color:#637381;font-size:12px;font-weight:600;font-family:Inter,sans-serif;text-align:center;line-height:18px;text-decoration:none\">Privacy policy</a><span style=\"color:#637381;padding:0 10px\">|</span><a href=\"\" style=\"color:#637381;font-size:12px;font-weight:600;font-family:Inter,sans-serif;text-align:center;line-height:18px;text-decoration:none\">Contact us</a></div></div></body>";
            }
            else if (type == "2")
            {
                body = "<head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><link href=\"https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap\" rel=\"stylesheet\"><title>Document</title></head><body><div class=\"email-bg\" style=\"max-width:600px;background:#f8fafc;margin:0 auto;padding:30px 0\"><div class=\"logo\" style=\"text-align:center;padding:10px 0\"><img src=\"https://dev-orderflow.foboh.com.au/static/media/fobohLogo.7e95493b401df09599df.png\" alt=\"\" style=\"max-width:150px\"></div><div class=\"inner-content\" style=\"max-width:500px;margin:10px 0;padding:20px 30px;background:#fff;margin:20px auto\"><h1 style=\"color:#147d73;font-size:32px;font-family:Inter,sans-serif;font-style:normal;font-weight:700;line-height:100%;letter-spacing:-.32px;margin-bottom:25px\">Reset your Password</h1><p style=\"font-family:Inter,sans-serif;font-size:15px;color:#637381;margin-bottom:25px\">Hi " + name + ",</p><p style=\"font-family:Inter,sans-serif;font-size:15px;color:#637381;margin-bottom:30px\"To reset your password, just click the button below.</p><div style=\"display:flex;justify-content:center\"><a href=\"" + url + "/auth/password-reset-form/" + key + "\" style=\"border-radius:8px;border:1px solid #f8fafc;background:#147d73;display:block;padding:6px 20px;text-decoration:none;color:#fff;font-family:Inter,sans-serif;margin:0 auto\">Reset Password</a></div><p style=\"font-size:12px;text-align:center;font-family:Inter,sans-serif;color:#637381;margin-top:20px\">Please note this link expires in 24 hours..</p><p style=\"font-size:15px;font-weight:600;color:#637381;font-family:Inter,sans-serif;margin-top:30px;margin-bottom:25px\">Too Late? <span style=\"font-size:16px;font-weight:400\">You can request another password rest </span><a href=\"\">here</a></p><p style=\"font-size:15px;font-weight:600;color:#637381;font-family:Inter,sans-serif;margin-top:30px;margin-bottom:25px\">Didn't request this email? <span style=\"font-size:16px;font-weight:400\">You don't need to do anything,your password will remain the same.</span></p><p style=\"font-size:15px;font-weight:600;color:#637381;font-family:Inter,sans-serif\">Having trouble?<a href=\"\" style=\"font-size:15px;font-weight:600;color:#147d73\"> Let us help you</a></p><p style=\"color:#637381;font-size:15px;font-weight:400;font-family:Inter,sans-serif;line-height:30px;margin-top:25px\">Thanks,<br>Team FOBOH</p></div><div class=\"bottom-footer\" style=\"text-align:center\"><p style=\"color:#637381;font-size:12px;font-weight:400;font-family:Inter,sans-serif;margin-top:60px;text-align:center;line-height:18px\">FOBOH Pty Ltd<br>ABN 47 668 157 739</p><a href=\"\" style=\"color:#637381;font-size:12px;font-weight:600;font-family:Inter,sans-serif;text-align:center;line-height:18px;text-decoration:none\">Terms & conditions</a><span style=\"color:#637381;padding:0 10px\">|</span><a href=\"\" style=\"color:#637381;font-size:12px;font-weight:600;font-family:Inter,sans-serif;text-align:center;line-height:18px;text-decoration:none\">Privacy policy</a><span style=\"color:#637381;padding:0 10px\">|</span><a href=\"\" style=\"color:#637381;font-size:12px;font-weight:600;font-family:Inter,sans-serif;text-align:center;line-height:18px;text-decoration:none\">Contact us</a></div></div></body>";
            }
            return body;
        }
        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
