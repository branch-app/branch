class HomeController < ApplicationController
	include Halo4::HomeHelper
	def index
		if (!current_user)
			redirect_to(root_path())
			return
		end

		@activity_feed = load_social_shit(0)
		#render json: @activity_feed
	end

	def welcome
		redirect_to(home_dashboard_path()) if (current_user)
	end

	private
		def load_social_shit(page)
			output = [ ]

			# get dem people you follow
			cool_guys = User.joins(:followers).where('follows.follower_id = ?', current_user().id).uniq

			# load social events
			cool_guys.each do |cool_guy|
				cool_guy.gamertag.social_event.order('created_at DESC, id DESC').limit(40).each do |social_event|
					if (social_event.is_h4_matchmaking_event?())
						h4_matchmaking_event = social_event.h4_matchmaking_event
						output << {
							event_date: h4_matchmaking_event.created_at,
							event_id: social_event.id,
							type: 'h4_matchmaking_event_single',
							content: {
								gamertag: cool_guy.gamertag,
								user: cool_guy,
								h4_content: h4_matchmaking_event
							}
						}
					end
				end
			end

			# favourites
			cool_guys.each do |cool_guy|
				cool_guy.favourite.order('created_at DESC, id DESC').limit(40).each do |favourite|
					output << {
						event_date: favourite.created_at,
						event_id: favourite.id,
						type: 'user_favourite',
						content: {
							user: cool_guy,
							favourite: favourite
						}
					}
				end
			end

			# followings
			index = 0
			cool_guys.each do |cool_guy|
				follows = Follow.where('follower_id = ?', cool_guy.id).order('created_at DESC, id DESC').limit(40)
				follows += Follow.where('following_id = ?', current_user().id).order('created_at DESC, id DESC').limit(40) if (index == 0)
				follows.each do |follow|
					new_obj = {
						event_date: follow.created_at,
						event_id: follow.id,
						type: 'user_follow',
						content: {
							user: cool_guy,
							follow: follow
						}
					}
					continue = true
					output.map{ |o| continue = false if (o[:type] == 'user_follow' && o[:event_id] == follow.id) }
					output << new_obj if (continue)
				end

				index += 1
			end
			puts output

			# Sort Everything Pre-Consolidating
			output = output.sort_by{ |o| o[:event_date] }.reverse

			# Consolidate Halo 4 Events
			cool_guys.each do |selected_cool_guy|
				current_ones = [ ]
				chain = 0
				last_time = nil
				last_playlist_id = -1
				output.each do |out|
					if (chain > 8)
						output = output.reject{ |c| current_ones.include?(c) }
						output << add_h4_event_multi(current_ones[0], current_ones)

						current_ones = [ ]
						chain = 0
					end

					if (out[:content][:user] == selected_cool_guy && 
						out[:type] == 'h4_matchmaking_event_single' &&
						(last_time == nil || last_time.to_i + 20.minutes.to_i >= out[:event_date].to_i) &&
						(last_playlist_id == -1 || last_playlist_id == out[:content][:h4_content].playlist_id))

						last_time = out[:event_date]
						last_playlist_id = out[:content][:h4_content].playlist_id
						current_ones << out
						chain += 1
					else
						if (chain > 1)
							# remove current_ones from output
							output = output.reject{ |c| current_ones.include?(c) }
							output << add_h4_event_multi(current_ones[0], current_ones)
						end
						current_ones = [ ]
						chain = 0
					end
				end

				if (chain > 1)
					output = output.reject{ |c| current_ones.include?(c) }
					output << add_h4_event_multi(current_ones[0], current_ones)
				end
			end

			# Sort again, and return shit
			return output.sort_by{ |o| o[:event_date] }.reverse.first(25)
		end

		def add_h4_event_multi(base_object, current_ones)
			new_obj = {
				event_date: base_object[:event_date],
				event_id: base_object[:event_id],
				type: 'h4_matchmaking_event_multi',
				content: {
					gamertag: base_object[:content][:gamertag],
					user: base_object[:content][:user],
					games: [ ]
				}
			}
			current_ones.map{ |c| new_obj[:content][:games] << c[:content][:h4_content] if (c[:content][:h4_content] != nil) }
			return new_obj
		end
end
