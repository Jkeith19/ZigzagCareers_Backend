using System.ComponentModel.DataAnnotations;

namespace Data.Entity.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public required string Title { get; set; }
        [Required]
        [StringLength(50)]
        public required string Author { get; set; }
        [Required]
        [StringLength(30)]
        public required string Isbn { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
    }
}
