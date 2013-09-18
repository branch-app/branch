class User::SessionController < User::HomeController
  def new
  end

  def create
  end

  def destroy
  	session[:user_id] = nil
  	reset_session

  	redirect_to(root_path)
  end
end
