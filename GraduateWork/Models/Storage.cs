using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp3.Models
{
    public class Storage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int QuantityWork { get; set; }

        public int Faulty { get; set; }

        public DateTime Date { get; set; }

        public string Supplier { get; set; }

        public int Price { get; set; }

        public string NameObject { get; set; }

        public int SalePrice { get; set; }

    }
}
