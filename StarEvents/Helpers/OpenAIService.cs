using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StarEvents.Helpers
{
    public static class OpenAIService
    {
        private static readonly string _apiKey;
        private static readonly string _model;
        private static readonly int _maxTokens;
        private static readonly double _temperature;

        static OpenAIService()
        {
            _apiKey = ConfigurationManager.AppSettings["OpenAI.ApiKey"];
            _model = ConfigurationManager.AppSettings["OpenAI.Model"] ?? "gpt-4o-mini";
            _maxTokens = int.Parse(ConfigurationManager.AppSettings["OpenAI.MaxTokens"] ?? "1000");
            _temperature = double.Parse(ConfigurationManager.AppSettings["OpenAI.Temperature"] ?? "0.7");
        }

        /// <summary>
        /// Sends a chat message to OpenAI API and returns the assistant's response.
        /// </summary>
        /// <param name="systemPrompt">System prompt containing context and instructions</param>
        /// <param name="userMessage">User's message/question</param>
        /// <returns>Assistant's response as a string</returns>
        public static async Task<string> ChatAsync(string systemPrompt, string userMessage)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return "Chat service is not configured. Please contact support.";

            try
            {
                using (var client = new HttpClient())
                {
                    // Configure HttpClient
                    client.BaseAddress = new Uri("https://api.openai.com/");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _apiKey);
                    client.Timeout = TimeSpan.FromSeconds(60); // Increased timeout to 60 seconds
                    
                    // Ensure connection is not kept alive to avoid hanging
                    client.DefaultRequestHeaders.ConnectionClose = false;

                    var payload = new
                    {
                        model = _model,
                        messages = new[]
                        {
                            new { role = "system", content = systemPrompt },
                            new { role = "user", content = userMessage }
                        },
                        max_tokens = _maxTokens,
                        temperature = _temperature
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make the API call with proper await
                    System.Diagnostics.Debug.WriteLine($"[OpenAI] Sending request to API: {_model}");
                    var response = await client.PostAsync("v1/chat/completions", content).ConfigureAwait(false);
                    
                    System.Diagnostics.Debug.WriteLine($"[OpenAI] Response status: {response.StatusCode}");
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        System.Diagnostics.Debug.WriteLine($"[OpenAI] Error response: {errorContent}");
                        return $"Sorry, I encountered an error communicating with the AI service (Status: {response.StatusCode}). Please try again later.";
                    }

                    var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    System.Diagnostics.Debug.WriteLine($"[OpenAI] Response received, length: {responseJson.Length}");
                    
                    var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                    
                    if (result?.choices == null || result.choices.Count == 0)
                    {
                        return "Sorry, I received an unexpected response from the AI service. Please try again.";
                    }
                    
                    return result.choices[0].message.content.ToString().Trim();
                }
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OpenAI] Timeout error: {ex.Message}");
                return "Sorry, the request timed out. Please try again with a shorter message.";
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[OpenAI] HTTP error: {ex.Message}");
                return $"Sorry, I couldn't connect to the AI service. Please check your internet connection and try again.";
            }
            catch (Exception ex)
            {
                // Log error but return user-friendly message
                System.Diagnostics.Debug.WriteLine($"[OpenAI] Exception: {ex.GetType().Name} - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[OpenAI] Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[OpenAI] Inner exception: {ex.InnerException.Message}");
                }
                return $"Sorry, I encountered an error: {ex.Message}. Please try again.";
            }
        }
    }
}
