# Issues with the legacy code

## Overview

As the code will be rewritten, it is crucial to follow best practices such as **splitting the code into multiple files** for better **organization, maintainability, and scalability**. Below is a breakdown of the issues found in the original codebase

## Critical Issues

1. **SQL Injection Vulnerabilities**:

   - **Issue**: The original code directly interpolates user input into SQL queries, leaving it vulnerable to SQL injection.
   - **Solution**: Use **parameterized queries** with an ORM.

2. **Hardcoded Secret Key**:

   - **Issue**: The secret key is hardcoded in the original code, which is insecure.
   - **Solution**: Store sensitive configurations (like the secret key) in environment variables or a **configuration file**

3. **Insecure Password Hashing (MD5)**:

   - **Issue**: The original code uses **MD5** for password hashing, which is insecure.
   - **Solution**: Use a more secure hashing algorithm like **bcrypt**, which provides secure password management out-of-the-box.

4. **Thread Safety with Database Connections**:

   - **Issue**: The `g.db` object in Flask is not thread-safe, which could lead to issues in concurrent environments.
   - **Solution**: Use **dependency injection** to manage the lifecycle of the database context in a thread-safe manner.
     
5. **Inefficient Database Usage**:
   - **Issue**: The code opens and closes a database connection for each request, which is inefficient for high-traffic applications.
   - **Solution**: Use a database connection pooling mechanism to manage and reuse connections across requests efficiently.

6. **Input Validation and Sanitization**:
   - **Issue**: Input validation is weak and does not sanitize inputs properly.

## High Priority Issues

7. **Lack of Error Handling in SQL Queries**:
   - **Issue**: SQL queries lack error handling, which could cause the application to crash.
   - **Solution**: Using an ORM.

## Medium Priority Issues

8. **No Centralized Error Logging**:

   - **Issue**: The application lacks centralized logging for errors and security events.

9. **Authentication not used**:
   - **Issue**: The logged in user is not needed to make requests to the search endpoint.
   - **Solution**: Consider saving search queries for a specific user, or removing it.

## Low Priority Issues

10. **No Pagination for Search Results**:
   - **Issue**: Search results are not paginated, which could affect performance with large datasets.
