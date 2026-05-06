# auth-session Specification

## Purpose

Defines the accepted backend authentication/session boundaries for API behavior in the template, including temporary verification scaffolding and deferred production session mechanics.

## Requirements

### Requirement: API Authentication Failures

API endpoints that require authentication MUST return `401 Unauthorized` for unauthenticated, missing, invalid, expired, or unmappable authentication contexts and MUST NOT redirect API requests to browser login pages.

#### Scenario: API request has no authenticated session

- **WHEN** an unauthenticated caller requests an API endpoint requiring
  authentication
- **THEN** the response is `401 Unauthorized` using standard API error
  semantics without a browser redirect
