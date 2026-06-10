namespace TraineeManagement.api.DTO.UserDto
{
    public class UserLoginRequestDto
    {
        public UserLoginRequestDto(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
