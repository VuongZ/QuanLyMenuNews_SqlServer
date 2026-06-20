using Application.DTO;

namespace Application.Interfaces;

public interface IMenuQueryRepository
{
   Task<MenuResponseDto?> GetByIdWithNewsAsync(int id);

    IAsyncEnumerable<MenuResponseDto> GetAllWithNewsAsync();
}