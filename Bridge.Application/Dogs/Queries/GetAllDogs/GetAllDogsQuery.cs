using AutoMapper;
using Bridge.Application.Common.Dtos;
using Bridge.Application.Common.Exceptions;
using Bridge.Domain.Entities.Dog;
using Bridge.Domain.Repositories;
using Bridge.Domain.RepositoriesContext;
using MediatR;

namespace Bridge.Application.Dogs.Queries.GetAllDogs;

//I dont see a point to write unit tests for this method since this handler is pretty straight forward
public record GetAllDogsQuery(
    string? Attribute,
    string Order,
    int PageNumber,
    int PageSize)
    : IRequest<IEnumerable<DogDto>>;

public class GetAllDogsQueryHandler : IRequestHandler<GetAllDogsQuery, IEnumerable<DogDto>>
{
    private readonly IDogRepository _dogsRepository;
    private readonly IMapper _mapper;

    public GetAllDogsQueryHandler(IDogRepository dogsRepository, IMapper mapper)
    {
        _dogsRepository = dogsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DogDto>> Handle(GetAllDogsQuery request, CancellationToken cancellationToken)
    {
        var sortingAttribute = GetSortingAttribute(request.Attribute);
        var sortingOrder = request.Order.ToLower() == GetAllDogsDefaultValues.DEFAULT_ORDER;
        var context = new GetDogsContext(sortingAttribute, sortingOrder, request.PageNumber, request.PageSize);
        var dogs = await _dogsRepository.GetDogsAsync(context, cancellationToken);
        var result = _mapper.Map<IEnumerable<DogDto>>(dogs);
        return result;
    }

    //if sorting attribute is not specified - dont perform the sorting
    //if sorting attribute is specified but not existent - throw an exception to notify the user
    private static DogSortOptions GetSortingAttribute(string? attribute)
    {
        if(attribute is null)
        {
            return DogSortOptions.None;
        }

        if(Enum.TryParse<DogSortOptions>(attribute, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new NonExistentSortAttribute(attribute);
    }
}
