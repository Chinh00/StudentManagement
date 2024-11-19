
FROM node:18-slim AS build

RUN apt-get update && apt-get install -y \
    build-essential \
    python3 \
    gcc \
    git \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /opt/app

COPY /src/UserService/user-api/package.json /src/UserService/user-api/yarn.lock ./

RUN yarn install --production

COPY /src/UserService/user-api/ .

RUN yarn build



FROM node:18-slim

RUN apt-get update && apt-get install -y libvips-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /opt/app

COPY --from=build /opt/app .

ENV NODE_ENV=development
EXPOSE 1337

CMD ["yarn", "run", "develop"]
