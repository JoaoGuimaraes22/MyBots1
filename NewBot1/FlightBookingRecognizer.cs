using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;

namespace MyCoreBotJG1
{
    public class FlightBookingRecognizer : IRecognizer
    {
        private readonly LuisRecognizer _recognizer;

        public FlightBookingRecognizer(IConfiguration configuration)
        {
            var luisIsConfigured = !string.IsNullOrEmpty(configuration["LuisAppId"]) && !string.IsNullOrEmpty(configuration["LuisAPIKey"]) && !string.IsNullOrEmpty(configuration["LuisAPIHostName"]);
            if (luisIsConfigured)
            {
                var luisApplication = new LuisApplication(
                    configuration["524d175b-d9d1-40c1-87bf-4819dbad2bcf"],
                    configuration["d5fc98e2f55e485c9ea61bc43bbc7d1d"],
                    "https://" + configuration["westus.api.cognitive.microsoft.com"]);

                _recognizer = new LuisRecognizer(luisApplication);
            }
        }

        // Returns true if luis is configured in the appsettings.json and initialized.
        public virtual bool IsConfigured => _recognizer != null;

        public virtual async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
            => await _recognizer.RecognizeAsync(turnContext, cancellationToken);

        public virtual async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken)
            where T : IRecognizerConvert, new()
            => await _recognizer.RecognizeAsync<T>(turnContext, cancellationToken);
    }
}
