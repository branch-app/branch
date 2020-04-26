import slugify from 'slugify';

export function toSlug(str: string): string {
	return slugify(str);
}

export function sanitise(str: string): string {
	return toSlug(str.trim());
}
