using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurfProjektAPI.Data.MapAPI;
using static SurfProjektAPI.Data.MapAPI.MapAPI;

namespace SurfProjektAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private HttpClient _httpClient;

        public MapController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("boards/{city}")]

        public async Task<ActionResult<List<MapDTO>>> GetLatAndLongFromCity(string city)
        {
            var response = await _httpClient.GetFromJsonAsync<Root>($"https://maps.googleapis.com/maps/api/geocode/json?address={city}&key=AIzaSyCjEsZPriND5V7v4VwrOG1GFVNA3mT0G8Y");
            List<MapDTO> mapDTOs = new List<MapDTO>();

            for (int i = 0; i < response.results.Count; i++)
            {
                mapDTOs.Add(new MapDTO
                {
                    lat = response.results[i].geometry.location.lat,
                    lng = response.results[i].geometry.location.lng,
                });
            }
            return mapDTOs;

        }



    }
}
