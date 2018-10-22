namespace ServiceRecord {
	export interface Specialization {
		id: number;
		name: string;
		description: string;
		imageUrl: string;
		level: number;
		completion: number;
		current: boolean;
		complete: boolean;
	}
}
