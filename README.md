# cuvva-log-sentry

Sentry integration for cuvva-log.

[![NPM Version](https://img.shields.io/npm/v/cuvva-log-sentry.svg?style=flat)](//www.npmjs.org/package/cuvva-log-sentry)
[![Build Status](https://img.shields.io/travis/cuvva/cuvva-log-sentry-node.svg?style=flat)](//travis-ci.org/cuvva/cuvva-log-sentry-node)
[![Coverage Status](https://img.shields.io/coveralls/cuvva/cuvva-log-sentry-node.svg?style=flat)](//coveralls.io/r/cuvva/cuvva-log-sentry-node)

```js
var log = require('cuvva-log');
var logSentry = require('cuvva-log-sentry');

var ravenClient; // existing Raven client

var sentryHandler = logSentry(ravenClient);
log.addHandler(sentryHandler);
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

log.addHandler('fatal', fatalHandler);
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

## License

MIT licensed - see [LICENSE](LICENSE) file
