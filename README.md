# cuvva-log

Super simple logging system used by all Cuvva systems

[![NPM Version](https://img.shields.io/npm/v/cuvva-log.svg?style=flat)](//www.npmjs.org/package/cuvva-log)
[![Build Status](https://img.shields.io/travis/cuvva/cuvva-log-node.svg?style=flat)](//travis-ci.org/cuvva/cuvva-log-node)
[![Coverage Status](https://img.shields.io/coveralls/cuvva/cuvva-log-node.svg?style=flat)](//coveralls.io/r/cuvva/cuvva-log-node)

```js
log.info('Hello, world!');
log.warn('some-problem', [innererror], { location:
    someLocation, occurence: occurenceCount });
throw log.error('something-serious');
```

## Current Status

Needs a way to integrate matching error codes to HTTP statuses but otherwise
mostly fully-featured.

Maybe needs a way to allow easier info logging without having to create a new
error code every time.

## Installation

```bash
$ npm install
```

## Usage

```js
// extensive set of usage examples
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
