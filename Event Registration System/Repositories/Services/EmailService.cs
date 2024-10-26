using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq.Expressions;

namespace EventRegistrationSystem.Repositories.Services
{
    public class EmailService
    {
        private readonly IConfiguration configuration;
        private readonly MailjetClient mailjetClient;

        public EmailService(IConfiguration Configuration)
        {
            configuration = Configuration;
            mailjetClient = new MailjetClient(
                 configuration["MailJet:ApiKey"],
                 configuration["MailJet:SecretKey"]
                );
        }
        public async Task<Boolean> SendEmail(string recipientEmail, string participantName, string nameEvent)
        {
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }.Property(Send.FromEmail, "ahmadraslan823@gmail.com").
             Property(Send.FromName, "Events")
             .Property(Send.Subject, "Registration Confirmation")
            .Property(Send.HtmlPart, $"<h3>Dear {participantName},</h3><p>You have successfully registered for the event Name {nameEvent}.</p><p>Thank you!</p>")
            .Property(Send.To, recipientEmail);
            MailjetResponse response = await mailjetClient.PostAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"st: {response.StatusCode }, info: {response.GetErrorInfo}");
            }
            return response.IsSuccessStatusCode;

        }

    }
}
