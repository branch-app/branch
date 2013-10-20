class Follow < ActiveRecord::Base
	attr_accessible :follower_id, :following_id, :follower, :following

	belongs_to :follower, class_name: 'User'
	belongs_to :following, class_name: 'User'

	validates :follower_id, presence: true
	validates :following_id, presence: true

	validate :cannot_follow_self
	validate :validate_follow_uniquiness

	def cannot_follow_self
		errors[:base] << 'You cannot follow yourself.' if (self.follower_id == self.following_id)
	end

	def validate_follow_uniquiness
		follow = Follow.find_by_follower_id_and_following_id(self.follower_id, self.following_id)
		errors[:base] << 'You cannot follow someone you already follow.' if (follow != nil)
	end
end
