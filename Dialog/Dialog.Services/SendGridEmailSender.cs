namespace Dialog.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Dialog.Common;
    using Dialog.Services.Contracts;
    using Dialog.ViewModels.SendGrid;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    // Documentation: https://sendgrid.com/docs/API_Reference/Web_API_v3/Mail/index.html
    public class SendGridEmailSender : IEmailSender
    {
        private const string AuthenticationScheme = "Bearer";
        private const string BaseUrl = "https://api.sendgrid.com/v3/";
        private const string SendEmailUrlPath = "mail/send";
        private readonly IConfiguration configuration;
        private readonly ISettingsService settingsService;
        private readonly ILogger logger;
        private readonly string apiKey;
        private readonly string fromAddress;
        private readonly string fromName;
        private readonly HttpClient httpClient;

        public SendGridEmailSender(IConfiguration configuration, ISettingsService settingsService, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.apiKey = this.configuration.GetSection("SendGrid").GetSection("ApiKey").Value;

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(AuthenticationScheme, apiKey);
            this.httpClient.BaseAddress = new Uri(BaseUrl);

            this.logger = loggerFactory.CreateLogger<SendGridEmailSender>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            this.fromAddress = this.settingsService.Get(GlobalConstants.SendEmailFromAdress);
            if (string.IsNullOrWhiteSpace(this.fromAddress))
            {
                throw new ArgumentOutOfRangeException(nameof(fromAddress));
            }
            this.fromName = this.settingsService.Get(GlobalConstants.SendEmailFromName);
            if (string.IsNullOrWhiteSpace(this.fromName))
            {
                throw new ArgumentOutOfRangeException(nameof(fromName));
            }
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(this.fromAddress))
            {
                throw new ArgumentOutOfRangeException(nameof(this.fromAddress));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentOutOfRangeException(nameof(email));
            }

            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Subject and/or message must be provided.");
            }

            var msg = new SendGridMessage(
                new SendGridEmail(email),
                subject,
                new SendGridEmail(this.fromAddress, this.fromName),
                message);
            try
            {
                var json = JsonConvert.SerializeObject(msg);
                var response = await this.httpClient.PostAsync(
                    SendEmailUrlPath,
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    // See if we can read the response for more information, then log the error
                    var errorJson = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"SendGrid indicated failure! Code: {response.StatusCode}, reason: {errorJson}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Exception during sending email: {ex}");
            }
        }
    }
}