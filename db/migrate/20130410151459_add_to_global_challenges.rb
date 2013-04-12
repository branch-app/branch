class AddToGlobalChallenges < ActiveRecord::Migration
  def up
		add_column :global_challenges, :name, :string
		add_column :global_challenges, :description, :string
		add_column :global_challenges, :category_id, :integer
		add_column :global_challenges, :category_name, :string
		add_column :global_challenges, :challenge_index, :integer
		add_column :global_challenges, :period_id, :integer
		add_column :global_challenges, :period_namely, :string
		add_column :global_challenges, :begin_date, :datetime
		add_column :global_challenges, :end_date, :datetime
		add_column :global_challenges, :required_count, :integer
		add_column :global_challenges, :game_mode_name, :string
		add_column :global_challenges, :xp_reward, :integer
		add_column :global_challenges, :required_skills, :integer
		add_column :global_challenges, :completed, :boolean
		add_column :global_challenges, :progress, :integer
  end

  def down
  end
end
