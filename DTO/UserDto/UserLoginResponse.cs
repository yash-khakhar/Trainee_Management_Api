namespace TraineeManagement.api.DTO.UserDto
{
    public class UserLoginResponse
    {
        
        public UserLoginResponse(string token, int expiresIn, UserResponse user)
        {
            Token = token;
            ExpiresIn = expiresIn;
            User = user;
        }

        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public UserResponse User {  get; set; }

    }
}
