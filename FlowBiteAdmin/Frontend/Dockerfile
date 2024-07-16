FROM node:20 AS base

WORKDIR /app

# By copying only the package.json and package-lock.json here, we ensure that the following `-deps` steps are independent of the source code.
# Therefore, the `-deps` steps will be skipped if only the source code changes.
COPY package.json package-lock.json ./

FROM base AS prod-deps
RUN npm install --omit=dev

FROM base AS build-deps
RUN npm install

FROM build-deps AS build
COPY . .

# Build the Astro project FOR DEVELOPMENT (default)
RUN npm run build

# Build the Astro project FOR PRODUCTION
# RUN npm run build -mode production

FROM base AS runtime
COPY --from=prod-deps /app/node_modules ./node_modules
COPY --from=build /app/dist ./dist

# Since we're running Astro in Hybrid mode, we need to copy the entire project directory to the runtime image.
COPY --from=build /app/ .

# ENV HOST=0.0.0.0
# ENV PORT=4321
# EXPOSE 80

# This is what will start our process. Making sure here to include the `--import` flag to include the instrumentation script.
CMD node --import /app/instrumentation.js /app/app.js