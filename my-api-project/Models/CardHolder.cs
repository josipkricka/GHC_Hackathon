using System.Collections.Generic;

namespace my_api_project.Models
{
    public class CardHolder
    {
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        public int CardHolderId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("first_name")]
        public string First { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("last_name")]
        public string Last { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("login_name")]
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public DateTime creation_time { get; set; }
        public DateTime last_login_time { get; set; }
        public int job_id { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

    }
}