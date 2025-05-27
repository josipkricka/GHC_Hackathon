namespace my_api_project.DTOs
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string TransDate { get; set; }
        public string TransTime { get; set; }
        public string Category { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string City { get; set; }
        public string Job { get; set; }
    }
}