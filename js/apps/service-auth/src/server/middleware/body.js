import { hasBody } from 'type-is';
import log from 'log';
import { Router as router } from 'express';
import { json, urlencoded } from 'body-parser';

const expressRouter = router();

export default expressRouter;

expressRouter.use(json());
expressRouter.use(urlencoded({ extended: true }));
expressRouter.use(checkBody);

function checkBody(req, res, next) {
	// has body, but wasn't parsed
	if (hasBody(req) && !req.body)
		next(log.info('unacceptable_content_type'));
	else
		next();
}
