using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaturalQuery.EntityFramework
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
