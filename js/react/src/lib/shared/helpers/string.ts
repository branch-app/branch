import slugify from 'slugify';

export function toSlug(str: string): string {
	return slugify(str);
}
