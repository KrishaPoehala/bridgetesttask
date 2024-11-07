using Bridge.Domain.Entities.Dog;

namespace Bridge.Domain.RepositoriesContext;

public record GetDogsContext(DogSortOptions? Attribute, bool Order, int PageNumber, int PageSize);
