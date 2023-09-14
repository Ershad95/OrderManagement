using FluentValidation;

namespace WebHost.ViewModels;
public class UpdateOrderVm
{
    public int ProductId { get; set; }
    public int PartId { get; set; }
    public int Id { get; set; }
}

public class UpdateOrderVmValidator : AbstractValidator<UpdateOrderVm>
{
    public UpdateOrderVmValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("لطفا شناسه درخواست را وارد کنید");
        RuleFor(x => x.PartId).NotEmpty().WithMessage("لطفا شناسه بخش را وارد کنید");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("لطفا شناسه محصول را وارد کنید");
    }
}
