# branch-log-sentry

Sentry integration for branch-log.

[![NPM Version](https://img.shields.io/npm/v/branch-log-sentry.svg?style=flat)](//www.npmjs.org/package/branch-log-sentry)
[![Build Status](https://img.shields.io/travis/branch-app/log-sentry-node.svg?style=flat)](//travis-ci.org/branch-app/log-sentry-node)
[![Coverage Status](https://img.shields.io/coveralls/branch-app/log-sentry-node.svg?style=flat)](//coveralls.io/r/branch-app/log-sentry-node)

```js
var log = require('@branch-app/log');
var logSentry = require('@branch-app/log-sentry');

var ravenClient; // existing Raven client

var sentryHandler = logSentry(ravenClient);
log.setHandler(sentryHandler);
```

## Installation

```bash
$ npm install
```

## Usage

Also supports taking a callback. This allows you to ensure logs have reached
Sentry before exiting:

```js
var fatalHandler = logSentry(ravenClient, function () {
	process.exit(1);
});

log.setHandler('fatal', fatalHandler);
```

## Testing

Install the development dependencies first:

```bash
$ npm install
```

Then the tests:

```bash
$ npm test
```

## Support

Please open an issue on this repository.

## Authors

- Jack Fransham
- Alex Forbes-Reed (adapted from Cuvva)

## License

MIT licensed - see [LICENSE](../../../LICENSE) file
