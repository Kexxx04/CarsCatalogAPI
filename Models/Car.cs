using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarsCatalog2.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "The model is obligatory.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The model must have between 2 and 100 characters.")]
        public string Model { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The price is obligatory.")]
        [Column(TypeName = "decimal(18,2)")]  // Especifica el tipo correcto para SQL Server
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The mileage of the auto is obligatory.")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "The brand is obligatory.")]
        public int BrandId { get; set; }

        [JsonIgnore] // Evita problemas de tracking en EF
        public Brand? Brand { get; set; }

    }
}
