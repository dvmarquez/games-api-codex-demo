# AGENTS.md

## Project Overview

This repository contains a .NET minimal API called GamesApi.

Structure:
- GamesApi.sln
- src/GamesApi (Web API project)
- tests/GamesApi.Tests (Automated tests)
- Dockerfile (Containerized deployment for Render)

The API manages an in-memory collection of video games and is designed as a learning proof of concept.

---

# Architecture & Design Principles

- Keep the implementation simple and beginner-friendly.
- Prefer clarity over cleverness.
- Avoid unnecessary abstraction or enterprise-level complexity.
- Do not introduce patterns unless explicitly requested.
- Keep business logic outside of Program.cs when reasonable.

---

# API Design Rules

When reviewing or generating endpoints:

- Use correct HTTP status codes:
  - GET → 200 OK
  - POST → 201 Created
  - Validation errors → 400 Bad Request
  - Not found → 404 Not Found
- Ensure responses are consistent and predictable.
- Avoid embedding validation logic directly inside endpoint delegates.
- Prefer request models over using domain models directly for POST/PUT.

---

# Validation Rules

- Use FluentValidation for input validation.
- Ensure validators are properly registered.
- Validation failures must return clear and structured 400 responses.
- Avoid silent failures.

---

# Testing Expectations

When new behavior is introduced:

- Add or update automated tests.
- Cover:
  - Happy path
  - Not found scenarios
  - Validation failures
- Prefer integration tests for endpoint behavior.
- Ensure tests are deterministic and independent.
- All tests must pass with `dotnet test`.

---

# Solution Structure

- Maintain separation:
  - src/ for application code
  - tests/ for test projects
- Ensure .sln includes all projects.
- Ensure test project references the API project correctly.
- Keep folder structure clean and organized.

---

# Docker & Deployment

- Dockerfile must use multi-stage build.
- The application must listen on port 8080.
- Avoid unnecessary image layers.
- Keep container size minimal when possible.
- Ensure compatibility with Render deployment.

---

# Code Review Guidelines

When reviewing pull requests:

- Prioritize correctness and clarity.
- Flag potential bugs and edge cases.
- Distinguish clearly between:
  - Critical issues
  - Improvements
  - Optional suggestions
- Avoid over-optimizing prematurely.
- Focus on high-confidence findings.

---

# Code Generation Guidelines

When generating new code:

- Follow the existing folder structure.
- Maintain consistency with current minimal API style.
- Keep methods concise and readable.
- Do not introduce breaking changes unless explicitly requested.
- Always consider test impact when modifying behavior.

---

# General AI Behavior Instructions

- Prefer simple, maintainable solutions.
- Avoid over-engineering.
- Highlight uncertainty explicitly.
- Provide actionable and concrete feedback.