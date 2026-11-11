# DevOps Deployment Project

A containerised deployment environment for an ASP.NET Core Web API, PostgreSQL database, and Nginx reverse proxy, hosted on an Ubuntu Server.

This project demonstrates practical DevOps concepts, including Linux server provisioning, Docker containerisation, multi-container orchestration, reverse proxy configuration, application deployment, server security, monitoring, and database backup and recovery.

## Project Overview

The objective of this project is to build and configure a Linux server environment capable of hosting a containerised web application using a maintainable and secure deployment architecture.

The application consists of three primary services:

- **ASP.NET Core Web API** – Handles application logic and exposes REST API endpoints.
- **PostgreSQL** – Stores application data using persistent Docker volumes.
- **Nginx** – Acts as a reverse proxy and provides an entry point for incoming HTTP requests.

The services are managed using Docker Compose.

## Architecture

```text
                 Client / Browser
                        |
                        | HTTP :80
                        v
              +-------------------+
              |   Ubuntu Server   |
              |                   |
              |   UFW Firewall    |
              +-------------------+
                        |
                        v
              +-------------------+
              |   Nginx Proxy     |
              |   Port: 80        |
              +-------------------+
                        |
                        | Reverse Proxy
                        v
              +-------------------+
              | ASP.NET Core API  |
              |   API Container  |
              +-------------------+
                        |
                        | Internal Docker Network
                        v
              +-------------------+
              |   PostgreSQL      |
              |   Database        |
              +-------------------+
                        |
                        v
                Persistent Volume
```

Only the Nginx service is intended to expose the application through the host's HTTP port. The API and database communicate through the internal Docker network.

## Technology Stack

| Category | Technology |
|---|---|
| Operating System | Ubuntu Server 26.10 |
| Backend | ASP.NET Core Web API |
| Database | PostgreSQL |
| Containerisation | Docker |
| Container Orchestration | Docker Compose |
| Reverse Proxy | Nginx |
| Firewall | UFW |
| Version Control | Git |
| CI/CD (planned) | GitHub Actions |
| Container Registry (planned) | GitHub Container Registry |

## Implemented Features

### 1. Linux Server Provisioning

- Installed and configured Ubuntu Server.
- Inspected server resources, CPU, memory, and disk usage.
- Created a dedicated deployment user.
- Configured sudo permissions.
- Applied Linux file and directory permissions.
- Configured UFW firewall rules.
- Verified network connectivity.

### 2. Docker Containerisation

- Containerised the ASP.NET Core Web API.
- Created Docker images using Dockerfiles.
- Applied multi-stage Docker build concepts.
- Configured container ports and networking.
- Managed container lifecycle operations.
- Used Docker Compose to manage multiple services.

### 3. Docker Compose Deployment

Configured the following services:

- ASP.NET Core API
- PostgreSQL database
- Nginx reverse proxy

Implemented:

- Service dependencies and health checks.
- Internal Docker networking.
- Persistent PostgreSQL storage using named volumes.
- Container lifecycle management through Docker Compose.

### 4. Nginx Reverse Proxy

- Configured Nginx as the application's reverse proxy.
- Forwarded incoming HTTP requests to the ASP.NET Core API.
- Configured security-related HTTP response headers.
- Disabled Nginx server version disclosure.
- Verified API accessibility through the reverse proxy.

### 5. Application and Database Configuration

- Externalised environment-specific configuration using an `.env` file.
- Protected environment configuration with restricted file permissions.
- Added sensitive files and backup directories to `.gitignore`.
- Configured PostgreSQL database credentials through environment variables.

### 6. Security and Production Readiness

Implemented initial server and container security practices:

- UFW firewall configuration.
- Restricted database and API exposure through Docker networking.
- Environment variable-based configuration.
- File permission restrictions for sensitive configuration.
- Nginx security headers.
- Docker container log rotation.
- Container health checks.

### 7. Monitoring and Logging

- Used `docker stats` to inspect container resource consumption.
- Used Docker Compose logs to investigate application and service issues.
- Configured Docker log rotation to prevent unlimited log growth.
- Verified container health and service status.

### 8. Database Backup and Recovery

- Created PostgreSQL database backups using `pg_dump`.
- Stored backup files locally.
- Restored a database backup into a separate PostgreSQL database.
- Verified restored data and application records.

## Application Endpoints

The API is accessible through the Nginx reverse proxy.

| Endpoint | Description |
|---|---|
| `http://SERVER_IP/health` | Checks API health |
| `http://SERVER_IP/api/products` | Retrieves product records |

Example health-check response:

```json
{
  "status": "Healthy",
  "service": "Product API"
}
```

Example product response:

```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 250000.00
  },
  {
    "id": 2,
    "name": "Keyboard",
    "price": 12000.00
  },
  {
    "id": 3,
    "name": "Mouse",
    "price": 5000.00
  }
]
```

## Getting Started

### Prerequisites

Ensure the deployment environment has:

- Ubuntu Server
- Docker Engine
- Docker Compose plugin
- Git
- Sufficient disk space and memory
- Network connectivity

### 1. Clone the Repository

```bash
git clone https://github.com/danufdo/devops-deployment.git

cd devops-deployment
```

### 2. Configure Environment Variables

Create the environment configuration file:

```bash
cp .env.example .env
```

Update the database credentials and other required environment variables according to the Docker Compose configuration.

Restrict access to the environment file:

```bash
chmod 600 .env
```

**Important:** Never commit `.env` files, passwords, access tokens, or other sensitive credentials to version control.

### 3. Build and Start the Services

Validate the Docker Compose configuration:

```bash
docker compose config
```

Build the application images and start the services:

```bash
docker compose up -d --build
```

### 4. Verify Deployment

Check the running containers:

```bash
docker compose ps
```

View application and service logs:

```bash
docker compose logs
```

Check API health:

```bash
curl http://localhost/health
```

Retrieve products:

```bash
curl http://localhost/api/products
```

### 5. Stop the Application

```bash
docker compose down
```

To remove containers and the associated named volumes (including persistent database data), use the following command only when data deletion is intended:

```bash
docker compose down -v
```

## Useful Docker Commands

| Command | Purpose |
|---|---|
| `docker ps` | Lists running containers |
| `docker images` | Lists available Docker images |
| `docker compose ps` | Displays Compose service status |
| `docker compose up -d` | Starts services in the background |
| `docker compose down` | Stops and removes Compose containers |
| `docker compose logs -f` | Follows service logs |
| `docker compose restart` | Restarts services |
| `docker stats` | Displays container resource usage |
| `docker volume ls` | Lists Docker volumes |
| `docker network ls` | Lists Docker networks |

## Repository Structure

The project is organised around the application source, Docker configuration, reverse proxy configuration, and deployment documentation.

```text
devops-deployment/
│
├── ProductApi/
│   ├── Dockerfile
│   └── ...
│
├── nginx/
│   └── ...
│
├── backups/
│
├── .github/
│   └── workflows/          # Future CI/CD implementation
│
├── .env                    # Local environment configuration
├── .env.example
├── .gitignore
├── docker-compose.yml
└── README.md
```

*The structure above is illustrative. Adjust directory names to match the actual repository.*

## Future Deliverables and Planned Implementations

The next phase of this project will focus on automating the build, validation, image publishing, and deployment processes.

### 1. CI/CD with GitHub Actions

**Status: Planned**

Implement a GitHub Actions pipeline to automate:

- Source code checkout.
- Application build and validation.
- Docker image building.
- Docker Compose configuration validation.
- Automated testing.
- Pipeline execution on pushes and pull requests.

### 2. Docker Image Publishing with GHCR

**Status: Planned**

Integrate GitHub Container Registry (GHCR) to:

- Publish application Docker images.
- Apply image tags based on Git commits or release versions.
- Maintain versioned container images.
- Enable deployment servers to pull application images from a central registry.

### 3. Automated Deployment

**Status: Planned**

Implement a deployment workflow to:

- Pull the latest approved application image.
- Update the running application containers.
- Perform deployment health checks.
- Verify application availability after deployment.
- Support rollback to a previous image version.

### 4. HTTPS and TLS Configuration

**Status: Planned**

- Configure HTTPS using Nginx.
- Integrate SSL/TLS certificates.
- Explore Let's Encrypt and Certbot.
- Redirect HTTP traffic to HTTPS.
- Review secure TLS configuration.

### 5. Enhanced Monitoring and Alerting

**Status: Planned**

Explore monitoring solutions such as:

- Prometheus for metrics collection.
- Grafana for dashboards.
- Alerting for service availability and resource usage.
- Centralised log management.

### 6. Infrastructure Automation

**Status: Planned**

Explore infrastructure automation using:

- Terraform.
- Ansible.
- Automated server provisioning.
- Repeatable deployment configuration.

### 7. Improved Backup and Disaster Recovery

**Status: Planned**

- Automate scheduled PostgreSQL backups.
- Implement encrypted off-server backup storage.
- Define backup retention policies.
- Test database recovery procedures.
- Document disaster recovery processes.

## Learning Outcomes

Through this project, the following practical DevOps concepts are being explored:

- Linux server administration.
- User and permission management.
- Firewall configuration and network troubleshooting.
- Docker image creation and container management.
- Multi-container deployment using Docker Compose.
- Reverse proxy configuration with Nginx.
- Application and database networking.
- Persistent storage management.
- Environment configuration and secret handling.
- Container monitoring and log management.
- Database backup and recovery.
- CI/CD pipeline design and implementation (planned).

## Project Status

<PRIVATE_STATUS_PLACEHOLDER>

**Current implementation:** Containerised application deployment, server configuration, reverse proxy, initial security hardening, monitoring, and database backup and recovery.

**Next milestone:** GitHub Actions CI pipeline and Docker image publishing to GHCR.

---

## Author

**Sonal Fernando**

Software Engineer | Technical Consultant | DevOps Enthusiast

- Portfolio: https://www.sonal.cloud/
- GitHub: https://github.com/danufdo

This project is part of my practical learning journey into Linux administration, containerisation, deployment automation, and DevOps engineering.
