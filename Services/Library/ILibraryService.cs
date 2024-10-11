using Data.Dto.Model;

namespace Services.Library
{
    public interface ILibraryService
    {
        Task<bool> AddBookAsync(AddBookDto addBookDto);
        Task<IEnumerable<BookDto>> GetBooksAsync();
        Task<BookDto?> UpdateBookAsync(int id, AddBookDto addBookDto);
        Task<BookDto> GetBookAsync(int id);
    }
}