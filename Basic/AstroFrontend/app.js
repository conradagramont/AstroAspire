import express from 'express';

// This page is generated by Astro during build time.
import { handler as ssrHandler } from './dist/server/entry.mjs';

// Added for Aspire specific configuration (Start) - Instrumentation
import { createTerminus, HealthCheckError } from '@godaddy/terminus';

import { env } from 'node:process';
import { createServer } from 'node:http';

const environment = process.env.NODE_ENV || 'development';
const app = express();
// The port below will be provided to us via Aspire AppHost.  We give it a default so the build doesn't fail.
const port = env.PORT ?? 8080;

// We'll get the environment variables that was injected by the Aspire AppHost in Basic\AstroAspire.AppHost\Program.cs
const apiServer = process.env['services__api__https__0'];
const apiBase = process.env['apiBasePath'];
const apiEndpoint = apiServer + apiBase;


// console.log(`env: ${env}`);
console.log(`apiServer: ${apiServer}`);
console.log(`apiBase: ${apiBase}`); 

// Change this based on your astro.config.mjs, `base` option.
// They should match. The default value is "/".
const base = '/';

// We'll use the ssrHandler as the middleware for all requests.
// We'll place this first, because we'll get a static file error in the tracing if we don't.
app.use(ssrHandler);
app.use(base, express.static('dist/client/'));

const server = createServer(app)

console.log(`environment: ${environment}`);
console.log(`apiServer: ${apiServer}`);
console.log(`apiURI: ${apiEndpoint}`);

// This is to support the health check for the Aspire AppHost, yet as defined in the ServiceDefaults project
async function healthCheck() {
    const errors = [];
    const apiServerHealthAddress = `${apiServer}/health`;

    console.log(`Fetching ${apiServerHealthAddress}`);

    var response = await fetch(apiServerHealthAddress);
    if (!response.ok) {
        throw new HealthCheckError(`Fetching ${apiServerHealthAddress} failed with HTTP status: ${response.status}`);
    }
}

// This is to support the health check for the Aspire AppHost, yet as defined in the ServiceDefaults project
createTerminus(server, {
    signal: 'SIGINT',
    healthChecks: {
        '/health': healthCheck,
        '/alive': () => { }
    },
    onSignal: async () => {
        console.log('server is starting cleanup');
        
        // Not using Redis, so commenting out. Saving for later.
        // console.log('closing Redis connection');
        // await cache.disconnect();
    },
    onShutdown: () => console.log('cleanup finished, server is shutting down')
});

// We're all set to go. Now we'll listen on the port.
server.listen(port, () => {
    console.log(`Listening on port ${port}`);
});