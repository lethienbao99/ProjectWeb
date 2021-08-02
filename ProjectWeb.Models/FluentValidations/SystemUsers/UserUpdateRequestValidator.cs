using FluentValidation;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.FluentValidations.SystemUsers
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is not empty")
                .MaximumLength(100).WithMessage("Fistname can not over 100 characters");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is not empty")
                .MaximumLength(100).WithMessage("LastName can not over 100 characters");

            RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("DateOfBirth can not greater than 100 years");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is not empty").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email is format not match");

        }
    }
}
