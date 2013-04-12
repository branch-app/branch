class CreateGlobalChallenges < ActiveRecord::Migration
  def change
    create_table :global_challenges do |t|

      t.timestamps
    end
  end
end
