using System.Collections.Generic;

namespace my_api_project.Models
{
    public class CardHolder
    {
        public int CardHolderId { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string City { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}