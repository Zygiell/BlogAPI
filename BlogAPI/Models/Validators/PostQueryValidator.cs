using BlogAPI.Entities;
using FluentValidation;

namespace BlogAPI.Models.Validators
{
    public class PostQueryValidator : AbstractValidator<PostQuery>
    {
        private string[] allowedSortByColumnNames = { nameof(Post.PostRating), nameof(Post.PostCreationDate) };

        public PostQueryValidator()
        {
            RuleFor(p => p.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by i s optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}