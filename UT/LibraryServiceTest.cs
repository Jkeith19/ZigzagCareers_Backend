using AutoMapper;
using Data.Dto.Mapper;
using Data.Dto.Model;
using Data.Entity.Context;
using Data.Entity.Model;
using Microsoft.EntityFrameworkCore;
using Services.Library;

namespace UT
{
    [TestFixture]
    public class Tests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private LibraryService _libraryService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

            _context = new ApplicationDbContext(options);
            var bookMapper = new BookMapper();
            _mapper = bookMapper.BookConfiguration();

            _libraryService = new LibraryService(_context, bookMapper);
        }

        [Test]
        public async Task GetBooksAsync_ReturnsMappedBooks()
        {
            // Arrange
            var books = new List<Book>()
            {
                new()
                {
                    Id = 0,
                    Title = "Book 1",
                    Author = "Test Author 1",
                    Isbn = "1234",
                    PublishedDate = DateTime.Now
                },
                new()
                {
                    Id = 0,
                    Title = "Book 2",
                    Author = "Test Author 2",
                    Isbn = "5678",
                    PublishedDate = DateTime.Now
                }
            }
            .AsQueryable();

            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();

            // Act
            var result = await _libraryService.GetBooksAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public async Task GetBookAsync_ReturnsMappedBook()
        {
            // Arrange
            var book = new Book()
            {
                Id = 0,
                Title = "Book 1",
                Author = "Test Author 1",
                Isbn = "1234",
                PublishedDate = DateTime.Now
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _libraryService.GetBookAsync(1);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task UpdateBookAsync_BookExists_UpdatesAndReturnsMappedBook()
        {
            // Arrange
            var existingBook = new Book()
            { 
                Id = 0,
                Title = "New Book",
                Author = "Test Author 1",
                Isbn = "1234",
                PublishedDate = DateTime.Now
            };
            await _context.Books.AddAsync(existingBook);
            await _context.SaveChangesAsync();

            var addBookDto = new AddBookDto()
            {  
                Title = "New Title",
                Author = "Test Author 1",
                Isbn = "1234",
                PublishedDate = DateTime.Now
            };

            // Act
            var result = await _libraryService.UpdateBookAsync(1, addBookDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Title, Is.EqualTo("New Title"));
        }

        [Test]
        public async Task UpdateBookAsync_BookDoesNotExist_ReturnsNull()
        {
            // Arrange
            var addBookDto = new AddBookDto()
            { 
                Title = "New Title",
                Author = "Test Author 1",
                Isbn = "1234",
                PublishedDate = DateTime.Now
            };

            // Act
            var result = await _libraryService.UpdateBookAsync(100, addBookDto);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddBookAsync_BookIsAddedSuccessfully_ReturnsTrue()
        {
            // Arrange
            var addBookDto = new AddBookDto()
            { 
                Title = "New Book",
                Author = "Test Author 1",
                Isbn = "123456789",
                PublishedDate = DateTime.Now
            };

            // Act
            var result = await _libraryService.AddBookAsync(addBookDto);

            // Assert
            Assert.IsTrue(result);
            var addedBook = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == "123456789");
            Assert.IsNotNull(addedBook);
            Assert.That(addedBook.Title, Is.EqualTo("New Book"));
        }

        [Test]
        public async Task AddBookAsync_BookWithExistingIsbn_ReturnsFalse()
        {
            // Arrange
            var existingBook = new Book()
            { 
                Id = 0,
                Title = "Existing Book",
                Author = "Test Author",
                Isbn = "123456789",
                PublishedDate = DateTime.Now
            };

            await _context.Books.AddAsync(existingBook);
            await _context.SaveChangesAsync();

            var addBookDto = new AddBookDto()
            { 
                Title = "New Book",
                Author = "Test Author",
                Isbn = "123456789",
                PublishedDate = DateTime.Now
            };

            // Act
            var result = await _libraryService.AddBookAsync(addBookDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}