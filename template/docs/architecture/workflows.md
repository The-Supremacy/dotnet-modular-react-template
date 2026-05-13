# Workflow Architecture

Workflow terminology should stay precise across product blueprints, runtime
code, durable execution graphs, and documentation.

- Intent is the user's desired product action.
- Classification is workflow assessment of message type, quality, routing
  confidence, or readiness.
- Artifacts are durable typed outputs created by workflow steps.
- Signals are routing triggers emitted by users, systems, classifiers, timers,
  or completed steps.
- Transitions route signals from one workflow state or step to another.
- Step input contracts declare which artifact types a step may access.

Product workflow blueprints and execution-framework graphs must not drift.
Prefer generating execution graphs from product blueprint/block definitions, or
generating both blueprint docs and execution graphs from a shared definition
source. When generation is not possible, the feature artifact must define the
review and test path that proves the blueprint and graph still match.

Durable product workflows should not assume one synchronous `RunAsync` call
returns the final product result. Model start, resume, status, cancellation,
timeout, and project-event boundaries explicitly. Long-running workflows should
persist state, expose status and artifact references, and publish project
events at meaningful transitions instead of hiding progress inside an in-memory
call stack.
