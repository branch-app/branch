# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)

roles = Role.create([
	{
		colour: '5cb85c',
		description: 'Developer',
		identifier: 3,
		name: 'A Developer on The Tree.'
	},
	{
		colour: '5bc0de',
		description: 'User',
		identifier: 2,
		name: 'A User of Branch App.'
	},
	{
		colour: '999999',
		description: 'Validating',
		identifier: 1,
		name: 'A Validating User of Branch App.'
	},
	{
		colour: 'd9534f',
		description: 'Banned',
		identifier: 0,
		name: 'A Banned User of Branch App.'
	}
])