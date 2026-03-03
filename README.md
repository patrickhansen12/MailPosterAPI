📨 Brevo Email Sender (.NET)

This project demonstrates how I implemented email sending in ASP.NET Core using the Brevo HTTP API.

The purpose of this project was to explore:

Secure configuration handling

External API integration

Whitelisting and recipient validation

Transition from SMTP to HTTP-based API calls

It is intentionally minimal and focused on backend implementation rather than UI, as the UI project is already made in Vue and just needs to be hooked up with the backend.

Frontend: https://github.com/patrickhansen12/mailposter

🚀 Features

Email sending via Brevo REST API (no SMTP (SMTP solution is in SMTP branch))

Environment-based configuration

Recipient whitelist validation

Clean separation of concerns (controller → service → config)

💻 Frontend project
This backend works together with the MailPoster frontend app.

Make sure to use the mailposter webapp https://github.com/patrickhansen12/MailPoster, for the best experience.



Things to note:
You need to insert your own Brevo API integration keys and email or use it together with another 3rd party client.
it is setup, so it should be extremely simple to implement it into your own workspace, as I havent used dependencies that was locked to Brevo api integration.
remember to add your own email on 3rd parties whitelist and in the app and not mine, if using the project.
Lastly many of the mail sites will be auto flagged as spam, so make sure to check your mail box in the spam folder, if it looks like your mail should have been sent.
