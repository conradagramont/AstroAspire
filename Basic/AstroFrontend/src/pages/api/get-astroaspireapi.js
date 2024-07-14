// Tell Astro not to prerender this page
export const prerender = false;

// Import the OpenTelemetry API
import OpenTelemetry from '@opentelemetry/api';
const tracer = OpenTelemetry.trace.getTracer('AstroAspire.Frontend');
const span = tracer.startSpan('Astro API - get-astroaspireapi.js');

// We'll get the environment variables that was injected by the Aspire AppHost in Basic\AstroAspire.AppHost\Program.cs
// 		- <snippet from Basic\AstroAspire.AppHost\Program.cs> -> <environment variable>
// 		- .WithReference(api)								->		services__api__https__0
// 		- .WithEnvironment("apiBasePath", apiBasePath)		->		apiBasePath
const weatherApiServer = process.env['services__api__https__0'];
const apiBase = process.env['apiBasePath'];
const apiEndpoint = weatherApiServer + apiBase;
const weatherAPIUrl = `${apiEndpoint}/weatherforecast`;

console.log(`   Hello from get-astroaspireapi.js`);

export async function GET() {
    span.setAttribute(`Hello from get-astroaspireapi.js`);
    span.setAttribute('weatherAPIUrl', weatherAPIUrl);
    
    console.log(`   Hello from get-astroaspireapi.js`);
    console.log("WeatherForecast URI: ", weatherAPIUrl);
    
    span.end();
    return new Response(
        // create a JSON object to return
        JSON.stringify({
            "weatherAspireApiUri": weatherAPIUrl
        })
    )
}

