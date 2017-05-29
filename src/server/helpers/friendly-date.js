import moment from 'moment-timezone';

export default function friendlyDate(date: string) {
	return moment(date).format('MMMM Do, YYYY');
}
