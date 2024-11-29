# How are you DevOps?

## Ways we are DevOps:

### Flow
- CI/CD pipeline with automated builds, tests, and deployments
    - Frontend and backend build/test workflows run on PRs
    - Deployment workflow triggers on tags
    - Monitoring stack deploys on manual trigger
- GitHub Flow branching strategy for less conflicts and to keep master branch clean
- Automated dependency management with Dependabot
- Containerized setup for consistent environments
- Kanban board for visualising and managing work/tasks

### Feedback
- Monitoring stack for realtime feedback of our system
    - Prometheus for metrics collection
    - Grafana for visualization
    - Node Exporter for system metrics
- Comprehensive testing
    - Backend unit tests
    - End-to-end testing with Playwright
    - Postman monitoring/integration tests
- Software quality checks
    - SonarCloud for static code analysis
    - Super-linter
- Structured feedback channels
    - Issue templates for bug reports and feature requests
    - PR template for easier overview when reviewing
    - GitHub badges for easy overview of status for deployments, tests, etc

### Continual Learning and Experimentation
- Documentation of process and postmortem(s).
- Focus on psychological safety
    - Emphasis on learning and sharing knowledge
- Experimentation with new tools and technologies, such as C# / .NET for our backend.

## Ways we are not DevOps:

### Deployment Strategy
- No zero-downtime deployment strategy implemented
    - Missing blue-green or canary deployment capabilities
    - All our deployments require some downtime, even though it might only be for short amounts of time
    
### Environment Management
- There is no automatic configuration for the dev environment, this is done manually
    - Potential inconsistencies between our development environments


### Missing Automation
- Logging in the backend is not implemented
- Monitoring is not implemented fully
  - Not all metrics are present (we plan to fix this in the future)
- There is no automatic alerting, should our system crash
- There is no automatic scaling yet, but this is something we've planned on trying to implement
- There is no automatic rollback setup in our system

### Future Improvements
- Implement logging in the backend, for easier debugging etc.
- More in depth metrics for our monitoring stack
- Implement automatic scaling
- Implement automatic rollback with f.x Helm
    - If our verify deployment workflow fails, automatically rollback to previous version
- Implement automatic alerts for our monitoring stack