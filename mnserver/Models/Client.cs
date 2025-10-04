using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mnserver.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto increment
        public int Id { get; set; }
        public int Kod { get; set; }
        public string Name { get; set; } = null!;
        public float Borcu { get; set; }
    }
}
