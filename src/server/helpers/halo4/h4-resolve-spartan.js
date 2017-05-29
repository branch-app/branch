export default function h4ResolveSpartan(identity, pose = 'fullbody', size = 'large') {
	return `https://spartans.svc.halowaypoint.com/players/${identity.gamertag}/h4/spartans/${pose}?target=${size}`;
}
