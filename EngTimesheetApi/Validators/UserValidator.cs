using EngTimesheetApi.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngTimesheetApi.Validators
{
	public class UserValidator : AbstractValidator<User>
	{
		public UserValidator()
		{
			RuleFor(x => x.Email).EmailAddress().WithMessage("Email must be a valid email");
			RuleFor(x => x.Email).Matches(@"\S+@onsolve\.com").WithMessage("Email must be an @onsolve.com email");
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName must not be empty");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName must not be empty");
		}
	}
}
