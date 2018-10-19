import bodyParser from 'body-parser';
import express from 'express';
import log from '@branch-app/log';
import typeIs from 'type-is';

const expressRouter = express.Router();

export default expressRouter;

expressRouter.use(bodyParser.json());
expressRouter.use(bodyParser.urlencoded({ extended: true }));
expressRouter.use(checkBody);

function checkBody(req, res, next) {
	// has body, but wasn't parsed
	if (typeIs.hasBody(req) && !req.body)
		next(log.info('unacceptable_content_type'));
	else
		next();
}
