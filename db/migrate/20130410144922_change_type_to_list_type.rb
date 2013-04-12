class ChangeTypeToListType < ActiveRecord::Migration
  def up
	  rename_column :services_lists, :type, :list_type
  end

  def down
  end
end
