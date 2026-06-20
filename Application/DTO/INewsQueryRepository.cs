namespace Application.DTO;
public interface INewsQueryRepository
{
            Task<NewsResponseDto?> GetByIdWithMenusAsync(int id);
      IAsyncEnumerable<NewsResponseDto> GetAllWithMenusAsync();
}