using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Intent;


namespace SpeechSDKWithLuis
{
    class Program
    {
        private const string LUIS_API_KEY = "TO REPLACE";
        private const string LUIS_API_SERVICE_REGION = "TO REPLACE";
        private const string LUIS_API_LANGUAGE_RECOGNITION = "TO REPLACE";
        private const string LUIS_APP_KEY = "TO REPLACE";

        async static Task Main(string[] args)
        {
            await RecognizeUserIntentAsync();

            Console.ReadLine();
        }

        private static async Task RecognizeUserIntentAsync()
        {
            var config = SpeechConfig.FromSubscription(LUIS_API_KEY, LUIS_API_SERVICE_REGION);
            config.SpeechRecognitionLanguage = LUIS_API_LANGUAGE_RECOGNITION;

            using (var recognizer = new IntentRecognizer(config))
            {
                var model = LanguageUnderstandingModel.FromAppId(LUIS_APP_KEY);
                recognizer.AddIntent(model, "TurnOffLights", "TurnOff");
                recognizer.AddIntent(model, "TurnOnLights", "TurnOn");
                recognizer.AddIntent(model, "GiveMeWeather", "Weather");

                Console.WriteLine("What can I do for you?");

                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                if (result.Reason == ResultReason.RecognizedIntent)
                {
                    var jsonResults = result.Properties.GetProperty(PropertyId.LanguageUnderstandingServiceResponse_JsonResult);
                    
                    // jsonResults can be parsed to find the entity
                    switch (result.IntentId)
                    {
                        case "TurnOff":
                            Console.WriteLine($"You said: {result.Text} so let me turn off the lights for you");
                            break;

                        case "TurnOn":
                            Console.WriteLine($"You said: {result.Text} so let me turn on the lights for you");
                            break;

                        //case "Weather":
                        //    break;
                    }

                    //Console.WriteLine($"Here is your JSON: {jsonResults}.");
                }
                else if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"You said: {result.Text} but no intent was recognized");
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine($"Sorry, I'm not able to understand what you want :(");
                }
            }
        }
    }
}
