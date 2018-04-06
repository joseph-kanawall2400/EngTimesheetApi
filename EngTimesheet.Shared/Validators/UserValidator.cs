using EngTimesheetApi.Shared.Models;
using FluentValidation;

namespace EngTimesheetApi.Shared.Validators
{
	public class UserValidator : AbstractValidator<User>
	{
		public UserValidator()
		{
			RuleFor(x => x.Email).EmailAddress().WithMessage("Email must be a valid email");
			RuleFor(x => x.Email).Matches(@"^\S+@onsolve\.com$").WithMessage("Email must be an @onsolve.com email");
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName must not be empty");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName must not be empty");
		}
	}
}
