using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsCatalog2.Models
{
    public class Brand
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        // Relación con los autos (una marca tiene muchos autos)
        public ICollection<Car> Cars { get; set; } = new List<Car>();


    }
}
