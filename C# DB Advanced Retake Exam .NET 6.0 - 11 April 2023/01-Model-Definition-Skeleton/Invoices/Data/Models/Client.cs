using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client() 
        { 
            ICollection<Invoice> invoices = new HashSet<Invoice>();
            ICollection<Address> addresses = new HashSet<Address>();
            ICollection<ProductClient> productsClients = new HashSet<ProductClient>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string NumberVat { get; set; }

        public virtual ICollection<Invoice> Invoices { get;}
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<ProductClient> ProductsClients { get; set; }
    }
}