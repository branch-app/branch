const httpNotFound = 404;

export default function (req, res): void {
	res.status(httpNotFound);
	res.send('page not found');
}
