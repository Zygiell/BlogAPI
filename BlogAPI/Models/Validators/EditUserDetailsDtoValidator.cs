using BlogAPI.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Models.Validators
{
    public class EditUserDetailsDtoValidator : AbstractValidator<EditUserDetailsDto>
    {        

        public EditUserDetailsDtoValidator()
        {
            
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .When(p => p.Password.Length > 0);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);
        }
    }
}
