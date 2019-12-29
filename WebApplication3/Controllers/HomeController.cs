using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public enum Decision {
        Yes = 0,
        No = 1
    }

    public class Model
    {
        public string UserName { get; set; }
        public Decision Decision { get; set; }
    }

    [AllowAnonymous]
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sendDecision")]
        public ActionResult SendDecision(Model result)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = "smtpotemkotest@gmail.com"; //Sender Email Address  
            string password = "!123456Q"; //Sender Password  
            string emailToAddress = "otemko@gmail.com"; //Receiver Email Address  
            string emailToAddress2 = ""; //Receiver Email Address  
            string subject = $"New Decision from {result.UserName}";
            var yesOrNo = result.Decision == Decision.No ? "No" : "Yes";
            string body = $"User {result.UserName} say {yesOrNo}";
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailToAddress);
                    mail.To.Add(emailToAddress2);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSL;

                        try
                        {
                            smtp.Send(mail);

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
            }

            return Json(new
            {
                msg = "Successfully added"
            });
        }
    }
}