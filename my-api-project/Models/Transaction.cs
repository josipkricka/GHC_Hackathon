using System;

namespace my_api_project.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransDate { get; set; }
        public string TransTime { get; set; }
        public string Category { get; set; }

        // Foreign keys
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }

        public int TransJobTypeId { get; set; }
        public TransJobType TransJobType { get; set; }
    }
}