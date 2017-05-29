export default function timer(req, res, next) {
	res.locals.startTime = new Date().getTime();

	next();
}
