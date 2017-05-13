import { Router as expRouter } from 'express';
import { hasBody } from 'type-is';
import inflector from 'json-inflector';
import log from '@branch-app/log';
import { json, urlencoded } from 'body-parser';

const router = expRouter();

export default router;

router.use(json());
router.use(urlencoded({ extended: true }));
router.use(checkBody);
router.use(inflector({ request: 'camelizeLower', response: 'underscore' }));

function checkBody(req, res, next) {
	// has body, but wasn't parsed
	if (hasBody(req) && !req.body)
		next(log.info('unacceptable_content_type'));
	else
		next();
}
