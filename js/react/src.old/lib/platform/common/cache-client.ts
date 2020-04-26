import { Response } from './types';
import uuid from 'uuid';
import idb, { DB } from 'idb';

const IDB_NAME = 'branch';
const IDB_STORE_NAME = 'cache';
const LOCAL_STORE_PREFIX = 'branch-app.cache';

// TODO(0xdeafcafe): Handle cleaning up of potentially orphaned items that had no matching
// sibling.

// TODO(0xdeafcafe): Handle some form of LRU cache logic to prevent local storage filling
// up.

interface Row {
	cachedAt: string;
	expiresAt: string;
	localStoreKey: string;
}

export default class CacheClient {
	private indexedDBSupported: boolean | null = null;
	private localStorageSupported: boolean | null = null;
	private storageSupported: boolean | null = null;

	private indexedDb: Promise<DB> = null;

	constructor() {
		this.checkCompatibility();

		if (!this.storageSupported)
			return;

		this.indexedDb = idb.open(IDB_NAME, 2, db => {
			db.createObjectStore(IDB_STORE_NAME);
		});
	}

	async set<T extends Response>(key: string, value: T) {
		if (!this.storageSupported)
			return;

		let localStoreKey = `${LOCAL_STORE_PREFIX}.${uuid.v4()}`;
		const tx = await this.createTransaction(false);
		const row: Row = await tx.get(key);

		if (row) {
			localStoreKey = row.localStoreKey;
		} else {
			await tx.put({
				localStoreKey,
				...value.cacheInfo, // Set cache info
			}, key);
		}

		window.localStorage.setItem(localStoreKey, JSON.stringify(value));
	}

	async get<T extends Response>(key: string, ignoreCacheChecks: boolean = false): Promise<T | null> {
		if (!this.storageSupported)
			return null;

		const tx = await this.createTransaction(true);
		const row: Row = await tx.get(key);

		if (!row)
			return null;

		const { expiresAt, localStoreKey } = row;

		if (ignoreCacheChecks)
			return this.readLocalStore(localStoreKey);

		if (new Date(expiresAt) > new Date())
			return this.readLocalStore(localStoreKey);

		return null;
	}

	async remove(key: string) {
		if (!this.storageSupported)
			return;

		const tx = await this.createTransaction(true);
		const row: Row = await tx.get(key);

		if (!row)
			return;

		await tx.delete(key);
		window.localStorage.removeItem(row.localStoreKey);
	}

	private async createTransaction(readOnly: boolean) {
		const db = await this.indexedDb;
		const tx = await db.transaction(IDB_STORE_NAME, readOnly ? 'readonly' : 'readwrite');

		return tx.objectStore(IDB_STORE_NAME);
	}

	private readLocalStore<T>(key: string): T | null {
		const str = window.localStorage.getItem(key);

		return str ? JSON.parse(str) as T : null;
	}

	private checkCompatibility() {
		const ls = this.checkLSCompatibility();
		const idb = this.checkIDBCompatibility();

		this.storageSupported = ls && idb;
	}

	private checkLSCompatibility(): boolean {
		if (this.localStorageSupported !== null)
			return this.localStorageSupported;

		if (typeof window.localStorage !== 'undefined') {
			try {
				window.localStorage.setItem('feature_test', 'yes');

				if (window.localStorage.getItem('feature_test') === 'yes') {
					window.localStorage.removeItem('feature_test');

					return (this.localStorageSupported = true);
				}
			} catch (error) { /**/ }
		}

		return (this.localStorageSupported = false);
	}

	private checkIDBCompatibility(): boolean {
		if (this.indexedDBSupported !== null)
			return this.indexedDBSupported;

		this.indexedDBSupported = (typeof window.indexedDB !== 'undefined');

		return this.indexedDBSupported;
	}
}
