using Application.DTO;
using MediatR;

namespace Application.Requests.XuLyAddress;

public class GetAllAddressRequest : IRequest<IEnumerable<WardInfoResponseDto>>
{
}
