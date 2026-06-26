using Application.DTO;
using MediatR;

namespace Application.Requests.XuLyAddress;

public class GetAddressByIdRequest : IRequest<WardInfoResponseDto?>
{
    public int id { get; set; }
}
