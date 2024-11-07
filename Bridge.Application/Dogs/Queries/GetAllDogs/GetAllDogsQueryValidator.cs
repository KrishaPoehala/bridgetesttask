using FluentValidation;

namespace Bridge.Application.Dogs.Queries.GetAllDogs;

public class GetAllDogsQueryValidator:AbstractValidator<GetAllDogsQuery>
{
	public GetAllDogsQueryValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThanOrEqualTo(0)
			.WithMessage("page number must be greater or equal to 0");
		RuleFor(x => x.Attribute)
			.Length(0, 100)
			.WithMessage("Sorting attribute's length must be in rango from 0 to 100");
		RuleFor(x => x.PageSize)
			.GreaterThan(0)
			.WithMessage("page size must be greater to 0");
    }
}