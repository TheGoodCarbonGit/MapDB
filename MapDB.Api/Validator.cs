using FluentValidation;
using MapDB.Api.DTOs;

namespace MapDB.Api
{
    public class postPinValidator : AbstractValidator<CreatePinDTO>
    {
        public postPinValidator()
        {
            RuleFor(pin => pin.Name).NotEmpty().MaximumLength(85);
            RuleFor(pin => pin.Category).NotEmpty();
            RuleFor(pin => pin.Description).NotEmpty();
            RuleFor(pin => pin.Coordinates).NotEmpty().SetValidator(new CoordinateValidator());
        }
    }

    public class putPinValidator : AbstractValidator<UpdatePinDTO>
    {
        public putPinValidator()
        {
            RuleFor(pin => pin.Name).NotEmpty().MaximumLength(85);
            RuleFor(pin => pin.Category).NotEmpty();
            RuleFor(pin => pin.Description).NotEmpty();
            RuleFor(pin => pin.Coordinates).NotEmpty().SetValidator(new CoordinateValidator());
        }
    }

    public class CoordinateValidator : AbstractValidator<double[]>
    {
        public CoordinateValidator()
        {
            RuleFor(coord => coord.Length).Equal(2);
            RuleFor(coord => coord[0]).InclusiveBetween(-90, 90);
            RuleFor(coord => coord[1]).InclusiveBetween(-180, 180);

        }
    }

}
