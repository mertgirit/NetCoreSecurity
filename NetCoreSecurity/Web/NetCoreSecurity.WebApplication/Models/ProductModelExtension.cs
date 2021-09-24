using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreSecurity.WebApplication.Models
{
    public partial class Product
    {
        [NotMapped]
        public string EncryptedId { get; set; }
    }
}