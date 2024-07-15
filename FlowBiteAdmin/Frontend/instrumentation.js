// Original source: https://github.com/dotnet/aspire-samples/blob/main/samples/AspireWithNode/NodeFrontend/instrumentation.js
import { env } from 'node:process';
import { NodeSDK } from '@opentelemetry/sdk-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-grpc';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-grpc';
import { OTLPLogExporter } from '@opentelemetry/exporter-logs-otlp-grpc';
import { SimpleLogRecordProcessor } from '@opentelemetry/sdk-logs';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { HttpInstrumentation } from '@opentelemetry/instrumentation-http';
import { ExpressInstrumentation } from '@opentelemetry/instrumentation-express';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
// import { ConsoleSpanExporter } from '@opentelemetry/sdk-trace-node';
// import { diag, DiagConsoleLogger, DiagLogLevel } from '@opentelemetry/api';
import { credentials } from '@grpc/grpc-js';
// import { url } from 'node:inspector';

const environment = process.env.NODE_ENV || 'development';

// For troubleshooting, set the log level to DiagLogLevel.DEBUG
// diag.setLogger(new DiagConsoleLogger(), environment === 'development' ? DiagLogLevel.INFO : DiagLogLevel.WARN);

const otlpServer = env.OTEL_EXPORTER_OTLP_ENDPOINT;

if (otlpServer) {
    console.log(`OTLP endpoint: ${otlpServer}`);

    const isHttps = otlpServer.startsWith('https://');
    const collectorOptions = {
        credentials: !isHttps
            ? credentials.createInsecure()
            : credentials.createSsl()
    };


    const sdk = new NodeSDK({
        traceExporter: new OTLPTraceExporter(collectorOptions),
        metricReader: new PeriodicExportingMetricReader({
            exportIntervalMillis: environment === 'development' ? 5000 : 10000,
            exporter: new OTLPMetricExporter(collectorOptions),
        }),
        logRecordProcessor: new SimpleLogRecordProcessor({
            exporter: new OTLPLogExporter(collectorOptions)
        }),
        instrumentations: [
            new HttpInstrumentation(),
            new ExpressInstrumentation(),
            // Configuration of below is done in AppHost
            // OTEL_NODE_RESOURCE_DETECTORS and OTEL_NODE_DISABLED_INSTRUMENTATIONS are key
            // For this project, we're only using OTEL_NODE_DISABLED_INSTRUMENTATIONS to remove the chatty fs instrumentation
            new getNodeAutoInstrumentations()
            
            // We're not using Redis in this project. Saving for later.
            // new RedisInstrumentation()
        ],
    });

    sdk.start();
} 