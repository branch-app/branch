const httpStatusNotAcceptable = 406;

export default function (req, res, next) {
	if (req.accepts('json'))
		next();
	else
		res.sendStatus(httpStatusNotAcceptable);
}
