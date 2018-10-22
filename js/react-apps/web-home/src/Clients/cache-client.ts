export default class CacheClient {
	private storageSupported: boolean | null = null;

	constructor() {
		this.checkCompatibility();
	}

	private checkCompatibility(): boolean {
		if (this.storageSupported !== null)
			return this.storageSupported;

		if (typeof window.localStorage !== 'undefined') {
			try {
				window.localStorage.setItem('feature_test', 'yes');

				if (window.localStorage.getItem('feature_test') === 'yes') {
					window.localStorage.removeItem('feature_test');

					return (this.storageSupported = true);
				}
			} catch (error) { /**/ }
		}

		return (this.storageSupported = false);
	}

	public writeValue<T extends Branch.Response>(key: string, value: T) {
		if (!this.storageSupported)
			return;

		window.localStorage.setItem(key, JSON.stringify(value));
	}

	public readValue<T extends Branch.Response>(key: string, ignoreCacheChecks: boolean = false): T | null {
		if (!this.storageSupported)
			return null;

		const strValue = window.localStorage.getItem(key);
		const value = strValue ? JSON.parse(strValue) as T : null;

		if (!value)
			return null;

		if (value && ignoreCacheChecks)
			return value;

		console.log(new Date(value.cacheInfo.expiresAt));
		console.log(new Date());
		console.log(new Date(value.cacheInfo.expiresAt) > new Date());

		if (new Date(value.cacheInfo.expiresAt) > new Date())
			return value;

		return null;
	}

	public removeValue(key: string) {
		if (!this.storageSupported)
			return;

		window.localStorage.removeItem(key);
	}
}
