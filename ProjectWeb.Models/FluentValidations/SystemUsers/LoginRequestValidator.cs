using FluentValidation;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.FluentValidations.SystemUsers
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is not empty");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is not empty")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");
        }
    }
}
