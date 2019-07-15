using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace PawsNClaws.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BusinessLocations()
        {
            ViewBag.Message = "Your locations page.";

            return View();
        }

        [HttpGet]
        public ActionResult Services()
        {
            ViewBag.Message = "Your services page.";

            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your about page.";

            return View();
        }

        [HttpGet]
        public ActionResult FAQ()
        {
            ViewBag.Message = "Your FAQ page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Models.ContactViewModel contactInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(contactInfo);
            }

            string body = string.Format(
                $"Name: {contactInfo.Name}<br />"
                + $"Email: {contactInfo.Email}<br/>"
                + $"Subject: {contactInfo.Subject}<br/>"
                + $"Message: {contactInfo.Message}<br/>");

            MailMessage msg = new MailMessage(
                "no-reply@jenmcook.com",
                "jenmcook@outlook.com",
                contactInfo.Subject, body);

            msg.IsBodyHtml = true;
            msg.CC.Add("jenmcook@outlook.com");

            SmtpClient client = new SmtpClient("mail.jenmcook.com");
            client.Credentials = new NetworkCredential("no-reply@jenmcook.com", "FakePassword");
            client.EnableSsl = false;
            client.Port = 8889;

            using (client)
            {
                try
                {
                    client.Send(msg);
                }
                catch
                {
                    ViewBag.ErrorMessage = "An error was encountered sending your message.\n"
                        + "Please try again.";
                    return View();
                }
            }
            return View("ContactConfirmation", contactInfo);
        }

    }
}
