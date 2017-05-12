export default function contentFor(name, options) {
	const blocks = this._blocks || (this._blocks = {});
	const block = blocks[name] || (blocks[name] = []);

	block.push(options.fn(this));
}
