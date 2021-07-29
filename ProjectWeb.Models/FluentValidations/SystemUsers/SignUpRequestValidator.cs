using FluentValidation;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.FluentValidations.SystemUsers
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is not empty")
                .MaximumLength(100).WithMessage("Fistname can not over 100 characters");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is not empty")
                .MaximumLength(100).WithMessage("LastName can not over 100 characters");

            RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("DateOfBirth can not greater than 100 years");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is not empty").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email is format not match");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is not empty");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is not empty")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.ConfirmPassword != request.Password)
                    context.AddFailure("Confirm password is not match");
            });

        }
    }
}
