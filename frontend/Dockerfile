FROM node:18-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

ARG VITE_DEV_API_BASE_URL
ARG VITE_MODE

RUN npm run build

FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/dist /usr/share/nginx/html

COPY nginx/nginx.conf /etc/nginx/nginx.conf

# Expose port 3000 (internal port)
EXPOSE 3000

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]