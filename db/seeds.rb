roles = Role.all
if Role.count == 0
	roles = Role.create([
		{
			colour: '5cb85c',
			name: 'Developer',
			identifier: 3,
			description: 'A Developer on The Tree.'
		},
		{
			colour: '5bc0de',
			name: 'User',
			identifier: 2,
			description: 'A User of Branch App.'
		},
		{
			colour: '999999',
			name: 'Validating',
			identifier: 1,
			description: 'A Validating User of Branch App.'
		},
		{
			colour: 'd9534f',
			name: 'Banned',
			identifier: 0,
			description: 'A Banned User of Branch App.'
		}
	])
end
