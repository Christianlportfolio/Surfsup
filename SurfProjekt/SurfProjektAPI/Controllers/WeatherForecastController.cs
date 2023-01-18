using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfProjektAPI.Data.WeatherForecastAPI;
using static SurfProjektAPI.Data.WeatherForecastAPI.WeatherForecastAPI;

namespace SurfProjektAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private HttpClient _httpClient;

        public WeatherForecastController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("/{city}")]

        public async Task<ActionResult<List<WeatherDTO>>> GetWeatherForecast(string city)
        {

            var response = await _httpClient.GetFromJsonAsync<Root>($"https://api.openweathermap.org/data/2.5/forecast?lang=da&q={city}&units=metric&appid=6bffa8dec9dc4800d583b41bddcdf65a");

            List<WeatherDTO> weatherDTOs = new List<WeatherDTO>();

            for (int i = 0; i < response.list.Count; i++)
            {

                weatherDTOs.Add(new WeatherDTO
                {
                    WindSpeed = response.list[i].wind.speed, 
                    WindDirection = response.list[i].wind.deg,
                    Temperature = response.list[i].main.temp,
                    Description = response.list[i].weather[0].description,
                    Time = DateTime.Parse(response.list[i].dt_txt),
                    City = city

                });

            }

            return weatherDTOs;

        }

    }
}
