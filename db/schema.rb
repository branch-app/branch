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

ActiveRecord::Schema.define(:version => 20130412003918) do

  create_table "authentication_vaults", :force => true do |t|
    t.text     "wlid_access_token"
    t.text     "wlid_authentication_token"
    t.datetime "wlid_expire"
    t.text     "spartan_token"
    t.datetime "created_at",                :null => false
    t.datetime "updated_at",                :null => false
  end

  create_table "game_meta_data", :force => true do |t|
    t.text     "data",       :limit => 2147483647
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "global_challenges", :force => true do |t|
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
    t.text     "data",       :limit => 2147483647
  end

  create_table "player_challenges", :force => true do |t|
    t.text     "data",       :limit => 2147483647
    t.string   "gamertag"
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "playlists", :force => true do |t|
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
    t.text     "data",       :limit => 2147483647
  end

  create_table "services_lists", :force => true do |t|
    t.string   "list_type"
    t.string   "name"
    t.text     "url"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

end
