FROM node:12.2.0

WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH

COPY film-api-ui/package.json /app/package.json
RUN  npm install -g @angular/cli

COPY film-api-ui/ ./app/

CMD ng serve
