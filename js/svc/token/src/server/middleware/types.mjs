export default function (req, res, next) {
	if (req.accepts('json'))
		next();
	else
		// eslint-disable-next-line no-magic-numbers
		res.sendStatus(406);
}
