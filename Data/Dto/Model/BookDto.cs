using System.ComponentModel.DataAnnotations;

namespace Data.Dto.Model
{
    public record BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Isbn { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
