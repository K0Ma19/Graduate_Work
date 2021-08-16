using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WpfApp3.Models
{
    public class Entrance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Supplier { get; set; }

        public int Price { get; set; }
    }
}
