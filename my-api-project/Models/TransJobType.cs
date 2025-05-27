
namespace my_api_project.Models
{
    public class TransJobType
    {
        public int TransJobTypeId { get; set; }
        public string Job { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}