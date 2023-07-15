using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.BL;
using NotificationApi.Models;

namespace NotificationApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [EnableCors("AllowSwaggerUI")]
    public class NotifyController : ControllerBase
    {
        private readonly ILogger<NotifyController> _logger;
        private readonly IConfiguration _configuration;
        public NotifyController(ILogger<NotifyController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("api/notify/sendmail")]
        public EmailResponse SendEmail(EmailData ed)
        {
            var auth = new Emailer(_configuration);
            var resp = auth.SendEMail(ed);
            return resp;
        }
    }
}
