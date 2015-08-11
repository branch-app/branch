'use strict';

const handlebars = require('handlebars');

const responses = {};
const catchers  = {
    debug: log,
    info:  log,
    warn:  log,
    error: log,
    fatal: log,
};

function logger(level) {
    return function (code, reasons, meta) {
        let err = createError(code, reasons, meta);

        propagateError(level, err);

        return err;
    };
}

function log(level, err) {
    // Pad so the codes of error/fatal line up with that of info and warn
    // TODO: print out inner errors instead of swallowing them (just verbose mode?)
    let errorString = message(level, err);

    console.log(errorString);
}

function message(level, err) {
    let errorString = `${pad(level, 5)}: ${err.code}`;
    const desc = description(err);

    if (desc)
        errorString += " -- " + desc;

    return errorString;
}

function pad(str, len) {
    while (str.length < len)
        str += " ";

    return str;
}

function description(err) {
    const response = responses[err.code];

    return typeof response === 'undefined' ? "" : response(err.meta);
}

function createError(code, reasons, meta) {
    if (!Array.isArray(reasons)) {
        meta = reasons;
        reasons = undefined;
    }

    return {
        code:       code,
        reasons:    reasons,
        meta:       meta,
    }
}

function addResponse(code, response) {
    responses[code] = handlebars.compile(response);
}

function addDescriptions(desc) {
    for (let prop in desc)
        responses[prop] = desc[prop];
}

function propagateError(level, err) {
    if (!(level in catchers))
        return;

    catchers[level](level, err);
}

function addCatch(level, catcher) {
    if (typeof catcher === 'undefined') {
        catcher = level;

        if (typeof catcher !== 'function')
            throw logger('error')('register-catch-error', { catcher: catcher });

        for (let prop in catchers)
        {
            catchers[prop] = catcher;
        }

        return;
    }

    if (typeof catcher !== 'function')
        throw logger('error')('register-catch-error', { catcher: catcher });

    catchers[level] = catcher;
}

module.exports = {
    info:         logger('info'),
    warn:         logger('warn'),
    debug:        logger('debug'),
    error:        logger('error'),
    fatal:        logger('fatal'),
    handler:      addCatch,
    create:       createError,
    description:  description,
    descriptions: setDescriptions
    message:      message,
};