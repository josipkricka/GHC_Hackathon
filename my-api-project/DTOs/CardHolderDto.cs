namespace my_api_project.DTOs
{
    public class CardHolderDto
    {
        public int CardHolderId { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string City { get; set; }
        public DateTime creation_time { get; set; }
        public DateTime last_login_time { get; set; }
        public int job_id { get; set; }
    }

    public class CreateCardHolderDto
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public int job_id { get; set; }
    }

    public class UpdateCardHolderDto
    {
        public int CardHolderId { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public int job_id { get; set; }
    }
}
