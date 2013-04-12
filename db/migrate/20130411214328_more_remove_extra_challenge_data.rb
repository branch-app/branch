class MoreRemoveExtraChallengeData < ActiveRecord::Migration
  def up
	  remove_column :global_challenges, :name
	  remove_column :global_challenges, :description
	  remove_column :global_challenges, :category_id
	  remove_column :global_challenges, :category_name
	  remove_column :global_challenges, :challenge_index
	  remove_column :global_challenges, :period_id
	  remove_column :global_challenges, :period_namely
	  remove_column :global_challenges, :begin_date
	  remove_column :global_challenges, :end_date
	  remove_column :global_challenges, :required_count
	  remove_column :global_challenges, :game_mode_name
	  remove_column :global_challenges, :xp_reward
	  remove_column :global_challenges, :required_skills
	  remove_column :global_challenges, :completed
	  remove_column :global_challenges, :progress
  end

  def down
	  add_column :global_challenges, :data, :text, :limit => 2147483647
  end
end
