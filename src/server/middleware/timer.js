export default function timer(req, res, next) {
	req.locals.startTime = new Date().getTime();

	next();
}
