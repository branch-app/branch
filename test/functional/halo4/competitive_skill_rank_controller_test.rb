require 'test_helper'

class Halo4::CompetitiveSkillRankControllerTest < ActionController::TestCase
  test "should get index" do
    get :index
    assert_response :success
  end

end
