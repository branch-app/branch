/* eslint-disable no-sync */

import fs from 'fs';
import log from '@branch-app/log';
import path from 'path';
import validator from 'is-my-json-valid';

const dirname = path.dirname(new URL(import.meta.url).pathname);

export default function createValidator(name) {
	const schema = fs.readFileSync(path.join(dirname, `${name}.json`));
	const validate = validator(JSON.parse(schema), { greedy: true });

	return input => {
		if (validate(input))
			return;

		const reasons = validate.errors.map(e => log.debug('invalid_field', e));

		throw log.info('invalid_body', reasons);
	};
}
