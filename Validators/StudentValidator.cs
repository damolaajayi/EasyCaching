using EasyCaching.Models;
using FluentValidation;

namespace EasyCaching.Validators
{
    internal sealed class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x=>x.FullName).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
        }
    }
}
