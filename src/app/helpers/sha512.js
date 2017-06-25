import crypto from 'crypto';

export function calculateHash(input: string, salt: string): string {
	const hash = crypto.createHmac('sha512', salt);

	hash.update(input);

	return hash.digest('hex');
}
