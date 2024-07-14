import { defineConfig } from 'astro/config';

import node from "@astrojs/node";

// https://astro.build/config
export default defineConfig({
  // Hybrid will focus more on static site generation, but will still allow for server-side rendering when needed
  // Reference: https://docs.astro.build/en/basics/rendering-modes/#on-demand-rendered
  output: "hybrid",
  // We'll use the node adapter to allow for server-side rendering
  // Reference: https://docs.astro.build/en/guides/integrations-guide/node/
  adapter: node({
    mode: "middleware"
  })
});