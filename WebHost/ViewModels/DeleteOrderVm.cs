using FluentValidation;

namespace WebHost.ViewModels;

public class DeleteOrderVm
{
    public int Id { get; set; }
}

public class DeleteOrderVmValidator : AbstractValidator<DeleteOrderVm>
{
    public DeleteOrderVmValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("لطفا شناسه درخواست را وارد کنید");
    }
}