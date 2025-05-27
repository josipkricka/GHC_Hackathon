namespace my_api_project.DTOs
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public CardHolderDto CardHolder { get; set; }
    }
}
