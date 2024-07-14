import OpenTelemetry from '@opentelemetry/api';

const tracer = OpenTelemetry.trace.getTracer('AstroAspire.Frontend');
// const span = tracer.startSpan('Astro Components - AspireRestWeather.js');

async function getWeatherData(WeatherAspireAPIUrl) {
    const span = tracer.startSpan('Astro Components - AspireRestWeather.js');

        console.log("function: getWeatherData-WeatherAspireAPIUrl: ", WeatherAspireAPIUrl);
        span.setAttribute('function: getWeatherData-WeatherAspireAPIUrl', WeatherAspireAPIUrl);
        try {
            
            // span.setAttribute('WeatherAspireAPIUrl', WeatherAspireAPIUrl);

            const response = await fetch(WeatherAspireAPIUrl);
            // console.log("response: ", response);

            const responseJson = await response.json();
            
            //console.log("responseJson: ", responseJson);

            return responseJson


        } catch (error) {
            console.error('Error fetching weather data:', error);
            // Be sure to end the span!
            /// span.end();
            throw error;
        }
        finally {
            // Be sure to end the span!
            span.end();
        }
}

export default getWeatherData;