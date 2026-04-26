using FitnessCenter.Application.Other;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Validators
{
    public class MembershipNewDtoValidator : AbstractValidator<MembershipNewDto>
    {
        public MembershipNewDtoValidator()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Клиент должен быть выбран");
            RuleFor(x => x.MembershipTypeId)
                .GreaterThan(0).WithMessage("Тип абонемента должен быть выбран");
            RuleFor(x => x.ActualCost)
                .GreaterThan(0).WithMessage("Стоимость абонемента должна быть больше нуля");
        }
    }
}
