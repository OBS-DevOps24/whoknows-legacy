# Service Level Agreement (SLA)

## 1. Overview
This Service Level Agreement (SLA) outlines the service commitments for the WhoKnows search engine application. The service consists of a frontend application, backend API, and database service.

## 2. Service Availability

| Category | Commitment |
|----------|------------|
| Target Uptime | 95% per month |
| Max Downtime | 36 hours per month |
| Maintenance Windows | Excluded from calculations |

### Known Limitations
- No zero-downtime deployment capability
- No automatic scaling implemented
- No automatic rollback capability
- Deployments require brief service interruptions

## 3. Performance Metrics

| Metric Type | Target | Success Rate |
|-------------|---------|--------------|
| API Response | < 2000ms | 90% of requests |


## 4. Monitoring & Reporting

| Component | Tools/Metrics |
|-----------|---------------|
| Collection | Prometheus, Node Exporter |
| Visualization | Grafana dashboards |
| Metrics Tracked | • CPU usage<br>• Memory utilization<br>• Request duration<br>• Error rates |
| Reporting | • Performance metrics in Grafana<br>• Post-incident reports<br>• No automated alerting |

## 5. Support Response Times

| Priority Level | Response Time |
|---------------|---------------|
| Critical | < 24 hours |
| High | < 48 hours |
| Standard | < 5 business days |
| Feature Requests | Best effort basis |

## 6. Security

| Category | Implementation |
|----------|---------------|
| Authentication | • JWT-based<br>• BCrypt password hashing<br>• HTTPS encryption |
| Compliance | • SonarCloud scans<br>• Automated dependency checks<br>• Secure configuration |

## 7. Exclusions
This SLA does not cover:
- Client-side issues
- Third-party service disruptions
- Planned maintenance windows
- Development environment issues
