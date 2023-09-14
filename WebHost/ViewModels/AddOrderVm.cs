using FluentValidation;

namespace WebHost.ViewModels;

public class AddOrderVm
{
    public int ProductId { get; set; }
    public int PartId { get; set; }
}

public class AddOrderVmValidator : AbstractValidator<AddOrderVm>
{
    public AddOrderVmValidator()
    {
        RuleFor(x => x.PartId).NotEmpty().WithMessage("لطفا شناسه بخش را وارد کنید");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("لطفا شناسه محصول را وارد کنید");
    }
}
