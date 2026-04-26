using FitnessCenter.Application.Other;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Validators
{
    public class MembershipTypeValidator : AbstractValidator<MembershipTypeDto>
    {
        public MembershipTypeValidator() 
        {
            RuleFor(x => x.MembershipName)
                .NotEmpty().WithMessage("Название абонемента обязательно")
                .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Стоимость должна быть больше нуля");
            RuleFor(x => x.PeriodDays)
                .GreaterThan(0).When(x => x.PeriodDays.HasValue)
                .WithMessage("Срок действия должен быть положительный числом");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов");
        }
    }
}
