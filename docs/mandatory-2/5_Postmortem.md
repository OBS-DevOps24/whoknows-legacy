# Postmortem

## Overview

Between November 26th and 28th, our database experienced an outage due to insufficient memory on the database VM. This resulted in all requests dependent on the database failing with status code 500. The incident was identified through our monitoring stack, which provided critical insights into the issue.

## Incident Timeline

- **November 26th:** The database began experiencing memory issues, leading to service disruptions.
- **November 27th:** Continued failures as the database container attempted to restart but failed with status code 137, indicating insufficient memory.
- **November 28th:** The issue persisted until manual intervention was performed.

## Visual Evidence

### Request Duration
![Request Duration](./assets/request-duration.png)

### Error Rate
![Error Rate](./assets/error-rate.png)

These graphs show the spike in request durations and error rates during the incident period, to the point of requests failing.

## Root Cause

The root cause of the outage was a lack of memory for the database VM.

## Resolution

- **Immediate Actions:**
  - Restarted the server and the database container, which seemed to work for a while, hence the normal data at a period between the 26th and 27th (and slow response time, once again).
  - Planned on making a swap memory to stabilize the virtual machine.

- **Long-term Solution:**
  - After repeated failure of the database, we decided to scale vertically, using a more powerful virtual machine (Azure B2s instance).

## Lessons Learned

- **Monitoring Effectiveness:**
  - Our monitoring stack was instrumental in identifying the issue.
  - However, the lack of automated alerting delayed our response time.

## How to prevent this in the future

1. **Implement Alerting:**
   - Set up Grafana alerts to notify us of critical issues, such as low memory, high CPU usage, etc.