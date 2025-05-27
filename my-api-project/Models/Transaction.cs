using System;

namespace my_api_project.Models
{
    public class Transaction
    {
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        public int TransactionId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("trans_num")]
        public string TransactionNumber { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("amount")]
        public decimal Amount { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("trans_date")]
        public DateTime TransDate { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("trans_time")]
        public string TransTime { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("category_id")]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        // Foreign keys
        [System.ComponentModel.DataAnnotations.Schema.Column("user_id")]
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
    }
}