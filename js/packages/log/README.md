# branch-log-node

Super simple logging system used by all of Branch's node services.

[![NPM Version](https://img.shields.io/npm/v/branch-log.svg?style=flat)](//www.npmjs.org/package/branch-log)
[![Build Status](https://img.shields.io/travis/branch-app/log-node.svg?style=flat)](//travis-ci.org/branch-app/log-node)
[![Coverage Status](https://img.shields.io/coveralls/branch-app/log-node.svg?style=flat)](//coveralls.io/r/branch-app/log-node)

```js
var log = require('branch-log');

log.debug('hello_world');

var reason1 = log.info('bad_email');
var reason2 = log.info('bad_phone');
log.warn('some_problem', [reason1, reason2], { foo: 'bar' });

throw log.error('something_serious');
```

## Installation

```bash
$ npm install
```

## Usage

If you have a traditional `Error` object, it can be coerced into a `BranchError`:

```js
var error; // existing traditional Error

var coerced = log.BranchError.coerce(error);
log.warn(coerced);
```

When traditional `Error` objects are provided as reasons, they're coerced automatically:

```js
var error; // existing traditional Error

log.warn('some_problem', [error]);
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
