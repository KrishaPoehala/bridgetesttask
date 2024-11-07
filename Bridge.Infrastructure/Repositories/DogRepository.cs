using Bridge.Domain.Entities.Dog;
using Bridge.Domain.Repositories;
using Bridge.Domain.RepositoriesContext;
using Bridge.Infrastructure.Extentions;
using Bridge.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bridge.Infrastructure.Repositories;

//to cover this class with tests it's better to use integrational testing(because this class uses DbContext 
//which is hard to mock)
//but the test task didn't require me to write those,so I'm gonna leave it as is
public class DogRepository : IDogRepository
{
    private readonly DogsContext _context;

    public DogRepository(DogsContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dog>> GetDogsAsync(GetDogsContext context, CancellationToken cancellationToken = default)
    {
        var query = _context.Dogs.AsQueryable();
        query = context.Attribute switch
        {
            DogSortOptions.None => query,
            DogSortOptions.Name => query.OrderBy(x => x.Name, context.Order),
            DogSortOptions.TailLength => query.OrderBy(x => x.TailLength, context.Order),
            DogSortOptions.Color => query.OrderBy(x => x.Color, context.Order),
            DogSortOptions.Weight => query.OrderBy(x => x.Weight, context.Order),
            _ => throw new UnreachableException()
        };

        query = context.PageNumber == 0 ? query :
            query.Skip(context.PageNumber * context.PageSize);

        query = context.PageSize == 0 ? query :
            query.Take(context.PageSize);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Dogs.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public void Add(Dog dog) => _context.Dogs.Add(dog);
}