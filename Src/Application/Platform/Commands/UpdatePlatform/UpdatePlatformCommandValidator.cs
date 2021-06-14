namespace Isitar.DependencyUpdater.Application.Platform.Commands.UpdatePlatform
{
    using Common;
    using FluentValidation;

    public class UpdatePlatformCommandValidator : AbstractValidator<UpdatePlatformCommand>
    {
        public UpdatePlatformCommandValidator()
        {
            Transform(x => x.Name, n => n.TrimToNull())
                .NotEmpty();

            RuleFor(x => x.GitUserEmail)
                .EmailAddress();
        }
    }
}