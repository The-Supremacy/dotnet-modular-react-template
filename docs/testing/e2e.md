# E2E Testing

End-to-end tests should cover controlled local-platform workflows once Aspire,
the Host, identity provider, Redis, PostgreSQL, Mailpit, and frontend apps are
runnable together.

E2E tests should not run by default until the full platform startup path is
stable enough for CI.

The default CI workflow does not start Aspire or automate the local
identity-provider browser login path. That scope needs a later accepted e2e
change with stable startup, credentials, and diagnostics.
