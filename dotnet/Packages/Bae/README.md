# Bae

Branch Application Exception (bae) is an exception format used across the codebase for errors that should trickle though requests. Non-Bae exceptions are sent to sentry but masked from users.

Yeah a `BaeException` suffers from [RAS syndrome](https://en.wikipedia.org/wiki/RAS_syndrome), I know.
