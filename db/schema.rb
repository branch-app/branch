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

ActiveRecord::Schema.define(:version => 20130923164304) do

  create_table "blog_categories", :force => true do |t|
    t.string   "name"
    t.string   "description"
    t.string   "slug"
    t.datetime "created_at",  :null => false
    t.datetime "updated_at",  :null => false
  end

  create_table "blog_posts", :force => true do |t|
    t.string   "name"
    t.string   "short_body"
    t.string   "full_body"
    t.integer  "user_id"
    t.boolean  "is_published"
    t.integer  "blog_category_id"
    t.datetime "created_at",       :null => false
    t.datetime "updated_at",       :null => false
    t.string   "slug"
  end

  create_table "gamertags", :force => true do |t|
    t.string   "gamertag"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "roles", :force => true do |t|
    t.string   "name"
    t.string   "description"
    t.string   "colour"
    t.integer  "identifier"
    t.datetime "created_at",  :null => false
    t.datetime "updated_at",  :null => false
  end

  create_table "sessions", :force => true do |t|
    t.string   "identifier"
    t.integer  "user_id"
    t.boolean  "expired"
    t.string   "owner_ip"
    t.string   "location"
    t.string   "gps_loc"
    t.string   "user_agent"
    t.string   "platform"
    t.string   "browser"
    t.string   "version"
    t.datetime "expires_at"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "user_verifications", :force => true do |t|
    t.integer  "user_id"
    t.string   "identifier"
    t.boolean  "has_verified"
    t.datetime "created_at",   :null => false
    t.datetime "updated_at",   :null => false
  end

  create_table "users", :force => true do |t|
    t.string   "username"
    t.string   "name"
    t.string   "email"
    t.string   "password"
    t.integer  "gamertag_id"
    t.integer  "role_id"
    t.datetime "created_at",  :null => false
    t.datetime "updated_at",  :null => false
  end

end
