using BlogAPI.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Models.Validators
{
    public class AdminEditUserDtoValidator : AbstractValidator<AdminEditUserDto>
    {
        private readonly BlogDbContext _dbContext;

        public AdminEditUserDtoValidator(BlogDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x =>  x.Email)                
                .EmailAddress()
                .When(e=> e.Email.Length > 0);

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .When(p => p.Password.Length > 0);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => new { x.Email, x.Id })
                .Custom((values, dbContext) =>
                {
                    var emailInUse = _dbContext.Users.Any(u => u.Email == values.Email && u.Id != values.Id);
                    if (emailInUse)
                    {
                        dbContext.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
