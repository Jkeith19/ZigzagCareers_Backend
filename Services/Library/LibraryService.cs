using AutoMapper;
using Data.Dto.Mapper;
using Data.Dto.Model;
using Data.Entity.Context;
using Data.Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Services.Library
{
    public class LibraryService : ILibraryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookMapper _bookMapper;
        private readonly IMapper _mapper;

        public LibraryService(ApplicationDbContext context, IBookMapper bookMapper)
        {
            _context = context;
            _bookMapper = bookMapper;
            _mapper = _bookMapper.BookConfiguration();
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var books = await _context.Books.ToListAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookAsync(int id)
        {
            var books = await _context.Books.FindAsync(id);

            return _mapper.Map<BookDto>(books);
        }

        public async Task<BookDto?> UpdateBookAsync(int id, AddBookDto addBookDto)
        {
            var checkBook = await _context.Books.AnyAsync(x => x.Id.Equals(id));

            if (!checkBook)
            {
                return null;
            }

            var book = _mapper.Map<Book>(addBookDto);
            book.Id = id;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> AddBookAsync(AddBookDto addBookDto)
        {
            var book = _mapper.Map<Book>(addBookDto);

            var checkBook = await _context.Books.AnyAsync(x => x.Isbn.Equals(book.Isbn));

            if (checkBook)
            {
                return false;
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
