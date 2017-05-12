export default function index(req, res): void {
	res.render('home/index', { title: 'Welcome to Branch!' });
}
