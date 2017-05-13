import { hasBody } from 'type-is';
import log from '@branch-app/log';
import { Router as router } from 'express';
import { urlencoded } from 'body-parser';

const r = router();

r.use(urlencoded({ extended: true }));
r.use((req, res, next) => {
	if (hasBody(req) && !req.body)
		next(log.info('unacceptable_content_type'));
	else
		next();
});

export default r;
