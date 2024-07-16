import { defineConfig } from 'astro/config';
import sitemap from '@astrojs/sitemap';
import tailwind from '@astrojs/tailwind';


import node from "@astrojs/node";

// This was hard coded in the FlowBite example. Let's get rid of this as soon as we can.
//const DEV_PORT = 4321;

const DEV_PORT = process.env.PORT || 4321;
// https://astro.build/config
export default defineConfig({
  
  site: process.env.CI
		? 'https://themesberg.github.io'
		: `http://localhost:${DEV_PORT}`,
	base: '/',

	output: "server",
	adapter: node({
		mode: "standalone"
	}),
  /*
  server: {
		// Dev. server only
		port: DEV_PORT,
	},
  */

	integrations: [
		//
		sitemap(),
		tailwind(),
	],
});