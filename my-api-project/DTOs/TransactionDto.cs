namespace my_api_project.DTOs
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int CardHolderId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class CreateTransactionDto
    {
        public int CardHolderId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class UpdateTransactionDto
    {
        public int TransactionId { get; set; }
        public int CardHolderId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
