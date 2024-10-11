using AutoMapper;
using Data.Dto.Model;
using Data.Entity.Model;

namespace Data.Dto.Mapper
{
    public class BookMapper : IBookMapper
    {
        public IMapper BookConfiguration()
        {
            return new MapperConfiguration(map =>
            {
                map.CreateMap<Book, BookDto>().ReverseMap();
                map.CreateMap<Book, AddBookDto>().ReverseMap();
            })
            .CreateMapper();
        }
    }
}
