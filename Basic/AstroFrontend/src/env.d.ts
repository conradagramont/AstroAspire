/// <reference types="astro/client" />

interface ImportMetaEnv {
    readonly apiURI: string;
    // more env variables...
}
  
interface ImportMeta {
    readonly env: ImportMetaEnv;
    readonly meta: ImportMetaEnv;
}

declare namespace App {
    interface Locals {
        apiServer: string
        apiPort: string
        apiURI: string
        environment: string
    }
  }

interface WeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}