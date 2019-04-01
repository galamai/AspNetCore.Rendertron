FROM node:8-slim as base
WORKDIR /app
EXPOSE 3000
RUN apt-get update && apt-get install -yq libgconf-2-4
RUN apt-get update && apt-get install -y wget --no-install-recommends \
    && wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
    && sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' \
    && apt-get update \
    && apt-get install -y google-chrome-stable fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst ttf-freefont \
      --no-install-recommends \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get purge --auto-remove -y curl \
    && rm -rf /src/*.deb

FROM base as src
WORKDIR /src
RUN apt-get update && apt-get install -y git
RUN git clone https://github.com/GoogleChrome/rendertron.git

FROM base as final
WORKDIR /app
COPY --from=src /src/rendertron .
COPY config.json .
RUN npm install && npm run build
ENTRYPOINT ["npm", "run", "start"]
