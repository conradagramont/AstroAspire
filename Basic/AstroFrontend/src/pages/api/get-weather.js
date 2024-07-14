// Tell Astro not to prerender this page
export const prerender = false;

// Let's point to our astro component that will make the API cal the Aspire weather API
import getWeatherData from '../../components/AspireRestWeather'


// Import the OpenTelemetry API
import OpenTelemetry from '@opentelemetry/api';
const tracer = OpenTelemetry.trace.getTracer('AstroAspire.Frontend');
//const span = tracer.startSpan('Astro API - api-get-weather.js');

// We'll get the environment variables that was injected by the Aspire AppHost in Basic\AstroAspire.AppHost\Program.cs
// 		- <snippet from Basic\AstroAspire.AppHost\Program.cs> -> <environment variable>
// 		- .WithReference(api)								->		services__api__https__0
// 		- .WithEnvironment("apiBasePath", apiBasePath)		->		apiBasePath
const weatherApiServer = process.env['services__api__http__0'];
const apiBase = process.env['apiBasePath'];
const apiEndpoint = weatherApiServer + apiBase;
const weatherAPIUrl = `${apiEndpoint}/weatherforecast`;



export async function GET() {
  // Set the tracer we'll use for this page
  
  // const tracer = OpenTelemetry.trace.getTracer('astro-frontend');
  // const span = tracer.startSpan('api\\get-weather.js');
  return tracer.startActiveSpan('Astro API - api-get-weather.js', async (span) => {

    console.log(`   weatherAPIUrl: ${weatherAPIUrl}`);
    console.log("WeatherForecast URI: ", weatherAPIUrl);
    
    span.setAttribute('weatherAPIUrl', weatherAPIUrl);
    
    span.addEvent('Calling getWeatherData() from AspireRestWeather.js');
    const thisWeather = await getWeatherData(weatherAPIUrl, span);
    span.addEvent('Calling getWeatherData() from AspireRestWeather.js');
    span.end();

    return new Response(
      JSON.stringify(thisWeather)
    )
  });
}

