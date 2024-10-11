using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dto.Model
{
    public record AddBookDto
    {
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Author { get; set; }
        [Required]
        public required string Isbn { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
    }
}
