
FROM node:18-slim AS build

RUN apt-get update && apt-get install -y \
    build-essential \
    python3 \
    gcc \
    git \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /opt/app

COPY /src/EnrollmentService/enrollment-api/package.json /src/EnrollmentService/enrollment-api/yarn.lock ./

RUN yarn install --production

COPY /src/EnrollmentService/enrollment-api/ .

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
