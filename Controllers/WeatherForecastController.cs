using Microsoft.AspNetCore.Mvc;

namespace MyBackstageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// TEST ###Gets the weather forecast.
    /// </summary>
    /// <returns>A list of weather forecast data.</returns>
    /// <response code="200">Returns the weather forecast</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    /// <summary>
    /// Adds a new WeatherForecast.
    /// </summary>
    /// <param name="forecast">The weather forecast to add.</param>
    /// <response code="201">Returns the newly created weather forecast.</response>
    /// <remarks>
    /// Sample request:
    /// 
    /// ```json
    /// POST /WeatherForecast
    /// {
    ///     "Date": "2022-01-01T00:00:00",
    ///     "TemperatureC": 23,
    ///     "TemperatureF": 74,
    ///     "Summary": "Sunny"
    /// }
    /// ``` 
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post([FromBody] WeatherForecast forecast)
    {
        // Logic to add the forecast
        return CreatedAtAction(nameof(Get), new { id = forecast.Date }, forecast);
    }

    /// <summary>
    /// Updates an existing WeatherForecast.
    /// </summary>
    /// <param name="date">The date of the weather forecast to update.</param>
    /// <param name="forecast">The updated weather forecast data.</param>
    /// <response code="200">Returns the updated weather forecast.</response>
    /// <response code="404">If the weather forecast is not found.</response>
    /// <remarks>
    /// Sample request:
    /// 
    /// ```json
    /// PUT /WeatherForecast/2022-01-01T00:00:00
    /// {
    ///     "Date": "2022-01-01T00:00:00",
    ///     "TemperatureC": 25,
    ///     "TemperatureF": 77,
    ///     "Summary": "Partly Cloudy"
    /// }
    /// ``` 
    /// </remarks>
    [HttpPut("{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Put(DateTime date, [FromBody] WeatherForecast forecast)
    {
        // Logic to update the forecast
        // For example, find the forecast by date and update it
        var existingForecast = FindForecastByDate(date);
        if (existingForecast == null)
        {
            return NotFound();
        }

        existingForecast.TemperatureC = forecast.TemperatureC;
        existingForecast.Summary = forecast.Summary;

        return Ok(existingForecast);
    }

    /// <summary>
    /// Deletes a weather forecast.
    /// </summary>
    /// <param name="date">The date of the weather forecast to delete.</param>
    /// <response code="204">Indicates the weather forecast was deleted</response>
    [HttpDelete("{date}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(DateTime date)
    {
        // Logic to delete the forecast
        return NoContent();
    }

    private WeatherForecast? FindForecastByDate(DateTime date)
    {
        // Dummy implementation to find a forecast by date
        // Replace with actual logic to find the forecast
        return new WeatherForecast
        {
            Date = date,
            TemperatureC = 20,
            Summary = "Sunny"
        };
    }
}
