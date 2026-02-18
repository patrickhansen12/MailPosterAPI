📨 Mailgun Email Sender (.NET)

This project demonstrates how I implemented email sending in ASP.NET Core using the Mailgun HTTP API.

The purpose of this project was to explore:

Secure configuration handling

External API integration

Whitelisting and recipient validation

Transition from SMTP to HTTP-based API calls

It is intentionally minimal and focused on backend implementation rather than UI, as the UI project is already made in Vue and just needs to be hooked up with the backend.

Frontend: https://github.com/patrickhansen12/mailposter

🚀 Features

Email sending via Mailgun REST API (no SMTP (SMTP solution is in SMTP branch))

Environment-based configuration

Recipient whitelist validation

Clean separation of concerns (controller → service → config)
