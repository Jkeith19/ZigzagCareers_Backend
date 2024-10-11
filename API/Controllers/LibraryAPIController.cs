using Data.Dto.Model;
using Microsoft.AspNetCore.Mvc;
using Services.Library;
using System.ComponentModel.DataAnnotations;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [ApiController]
    public class LibraryAPIController : ControllerBase
    {
        private readonly ILibraryService _libraryService;
        public LibraryAPIController(ILibraryService libraryService) 
        { 
            _libraryService = libraryService;
        }

        [HttpGet]
        [Route("books")]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var books = await _libraryService.GetBooksAsync();
                return StatusCode(200, books);
            }
            catch
            {
                //ExceptionMessage consists of sensitive data so I used custom error message instead
                return StatusCode(500, "Internal Service Error");
            }
        }

        [HttpPost]
        [Route("book")]
        [ProducesResponseType(typeof(BookDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddAsync([Required][FromBody] AddBookDto book)
        {
            try
            {
                var result = await _libraryService.AddBookAsync(book);

                if (result != true)
                {
                    return StatusCode(400);
                }

                return StatusCode(201);
            }
            catch
            {
                //ExceptionMessage consists of sensitive data so I used custom error message instead
                return StatusCode(500, "Internal Service Error");
            }
        }

        [Route("book/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(BookDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var book = await _libraryService.GetBookAsync(id);

                if (Equals(book, null))
                {
                    return StatusCode(404);
                }
                return StatusCode(200, book);
            }
            catch
            {
                //ExceptionMessage consists of sensitive data so I used custom error message instead
                return StatusCode(500, "Internal Service Error");
            }
        }

        [Route("book/{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(BookDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateAsync(int id, [Required][FromBody] AddBookDto addBookDto)
        {
            try
            {
                var book = await _libraryService.UpdateBookAsync(id, addBookDto);

                if (Equals(book, null))
                {
                    return StatusCode(404);
                }

                return StatusCode(200, book);
            }
            catch
            {
                //ExceptionMessage consists of sensitive data so I used custom error message instead
                return StatusCode(500, "Internal Service Error");
            }
        }
    }
}
