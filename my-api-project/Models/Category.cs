

namespace my_api_project.Models
{
    public class Category
    {
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        public int CategoryId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        public string Name { get; set; }
        
        public ICollection<Transaction> Transactions { get; set; }
    }
}