# cuvva-log

Super simple logging system used by all Cuvva systems

[![NPM Version](https://img.shields.io/npm/v/cuvva-log.svg?style=flat)](//www.npmjs.org/package/cuvva-log)
[![Build Status](https://img.shields.io/travis/cuvva/cuvva-log-node.svg?style=flat)](//travis-ci.org/cuvva/cuvva-log-node)
[![Coverage Status](https://img.shields.io/coveralls/cuvva/cuvva-log-node.svg?style=flat)](//coveralls.io/r/cuvva/cuvva-log-node)

```js
var log = require('cuvva-log');

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
