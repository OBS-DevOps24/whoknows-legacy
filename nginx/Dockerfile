# Use the official Nginx image from the Docker Hub
FROM nginx:latest

# Remove the default Nginx configuration
RUN rm /etc/nginx/conf.d/default.conf

# Create necessary directories if they don't exist
RUN mkdir -p /run/nginx


# Copy your custom Nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80
EXPOSE 80