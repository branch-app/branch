import { Binary } from 'mongodb';
import MUUID from 'mongo-uuid';
import log from '@branch-app/log';

export default {
	generate,
	parse,
	stringify,
};

function generate(): {} {
	return MUUID.create(Binary);
}

function parse(str): ?{} {
	if (str === void 0 || str === null)
		return null;

	if (str instanceof Binary && str.sub_type === Binary.SUBTYPE_UUID)
		return str;

	try {
		return MUUID.parse(str);
	} catch (error) {
		if (error.name === 'ParseError')
			throw log.info('invalid_id');
		throw error;
	}
}

function stringify(obj): ?string {
	if (obj === void 0 || obj === null)
		return null;

	if (obj instanceof String)
		return obj;

	return MUUID.stringify(obj);
}
