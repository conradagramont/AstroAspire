/// <reference types="astro/client" />

interface ImportMetaEnv {
    readonly apiURI: string;
    readonly SITE: string;
    // more env variables...
}
  
interface ImportMeta {
    readonly env: ImportMetaEnv;
    //readonly meta: ImportMetaEnv;
}