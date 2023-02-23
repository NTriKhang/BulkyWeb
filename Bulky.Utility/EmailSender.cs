using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var mail = "nguyentrikhang1703@gmail.com";
			var pw = "kzcupqfoxjrkmozs";
			MailMessage msg = new MailMessage();

			string[] toAddressList = email.Split(';');

			//Loads the To address field 
			foreach (string toaddress in toAddressList)
			{
				if (toaddress.Length > 0)
				{
					msg.To.Add(toaddress);
				}
			}

			msg.From = new MailAddress(mail, "Test");
			msg.Subject = subject;
			msg.Body = htmlMessage;
			msg.IsBodyHtml = true;


			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw),
			};

			return client.SendMailAsync(msg);
		}
	}
}
