# encoding: UTF-8
# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended to check this file into your version control system.

ActiveRecord::Schema.define(:version => 20130415113100) do

  create_table "h4_api_authentication_vaults", :force => true do |t|
    t.text     "wlid_access_token"
    t.text     "wlid_authentication_token"
    t.datetime "wlid_expire"
    t.text     "spartan_token"
    t.datetime "created_at",                :null => false
    t.datetime "updated_at",                :null => false
  end

  create_table "h4_game_metadata", :force => true do |t|
    t.text     "data",       :limit => 2147483647
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "h4_global_challenges", :force => true do |t|
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
    t.text     "data",       :limit => 2147483647
  end

  create_table "h4_player_challenges", :force => true do |t|
    t.text     "data",       :limit => 2147483647
    t.string   "gamertag"
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "h4_player_commendations", :force => true do |t|
    t.string   "gamertag"
    t.text     "data",       :limit => 2147483647
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "h4_player_matches", :force => true do |t|
    t.string   "gamertag"
    t.text     "data",       :limit => 2147483647
    t.string   "game_id"
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "h4_player_recent_matches", :force => true do |t|
    t.string   "gamertag"
    t.text     "data",        :limit => 2147483647
    t.datetime "created_at",                        :null => false
    t.datetime "updated_at",                        :null => false
    t.integer  "start_index"
    t.integer  "count"
    t.integer  "mode_id"
    t.integer  "chapter_id"
  end

  create_table "h4_player_servicerecords", :force => true do |t|
    t.string   "gamertag"
    t.text     "data",       :limit => 2147483647
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "h4_playlists", :force => true do |t|
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
    t.text     "data",       :limit => 2147483647
  end

  create_table "h4_services_lists", :force => true do |t|
    t.string   "list_type"
    t.string   "name"
    t.text     "url"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

end
