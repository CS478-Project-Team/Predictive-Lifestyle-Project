using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Diagnostics;

namespace SendingEmails
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			Debug.WriteLine("sendEmailAsync called");

			try
			{
				var emailMessage = new MimeMessage();

				// From address
				emailMessage.From.Add(new MailboxAddress(
					"Predictive App",
					"predictiveapp478@gmail.com"));

				// To address
				emailMessage.To.Add(new MailboxAddress("", email));

				// Subject
				emailMessage.Subject = subject;

				// Body
				var bodyBuilder = new BodyBuilder
				{
					HtmlBody = message // or use TextBody for plain text
				};
				emailMessage.Body = bodyBuilder.ToMessageBody();

				// Send email
				using (var client = new SmtpClient())
				{
					// Connect to Outlook SMTP server
					await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

					// Authenticate
					await client.AuthenticateAsync(
						"predictiveapp478@gmail.com",
						"bcuc xhsv zfve yafw "); //app password

					// Send message
					await client.SendAsync(emailMessage);

					// Disconnect
					await client.DisconnectAsync(true);

					Console.WriteLine($"Email sent successfully to {email}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error sending email: {ex.Message}");
				Console.WriteLine($"StackTrace: {ex.StackTrace}");
				throw;
			}
		}
	}
}

/*using System.Net.Mail;
using System.Net;
using System.Diagnostics;

//namespace SendingEmails
{
	public class EmailSender : IEmailSender
{
	public async Task SendEmailAsync(string email, string subject, string message)
	{
		Debug.WriteLine("sendEmailAsync called");

		var mail = "PredictiveApp478@outlook.com";
		var pw = "gmbtwuhcobexuyrn";

		var client = new SmtpClient("smtp-mail.outlook.com", 587)
		{
			Credentials = new NetworkCredential(mail, pw),
			EnableSsl = true
		};

		try
		{
			var mailMessage = new MailMessage(from: mail, to: email, subject, message)
			{
				IsBodyHtml = true // optional, in case your message is HTML
			};

			await client.SendMailAsync(mailMessage);
			Console.WriteLine($"Email sent successfully to {email}");
		}
		catch (SmtpException smtpEx)
		{
			// Specific SMTP-level error (authentication, connection, etc.)
			Console.WriteLine($"SMTP error sending email: {smtpEx.Message}");
			Console.WriteLine($"StatusCode: {smtpEx.StatusCode}");
			throw; // Re-throw if you want the caller to handle it too
		}
		catch (Exception ex)
		{
			// General fallback
			Console.WriteLine($" Unexpected error sending email: {ex.Message}");
			throw;
		}

		//return client.SendMailAsync(
		//	new MailMessage(from: mail, 
		//					to: email, 
		//					subject,
		//					message));

	}
}*/
//}

