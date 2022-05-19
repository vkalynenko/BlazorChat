using BlazorChatApp.DAL.Domain.EF;

namespace BlazorChatApp.PL.Data
{

    public class WeatherForecastService
    {
        private readonly BlazorChatAppContext _appContext;  
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public WeatherForecastService(BlazorChatAppContext appContext)
        {
            _appContext = appContext;
        }

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            //var id = 1;
            //var chat = _appContext.Chats.Find(id);
            //var name = chat.ChatName;
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }

       
    }
}