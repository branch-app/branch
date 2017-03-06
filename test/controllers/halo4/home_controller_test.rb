require 'test_helper'

class Halo4::HomeControllerTest < ActionDispatch::IntegrationTest
  test "should get index" do
    get halo4_home_index_url
    assert_response :success
  end

end
