namespace SurfProjektAPI.Data.WeatherForecastAPI
{
    //DTO klassen sørger for, at vi sender relevant data videre. Står for data tranfer object.
    public class WeatherDTO
    {
        public int WindDirection { get; set; }

        public double WindSpeed { get; set; }

        public double Temperature { get; set; }

        public string Description { get; set; }

        public DateTime Time { get; set; }

        public string City { get; set; }
    }
}
