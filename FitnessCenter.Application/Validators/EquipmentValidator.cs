using FitnessCenter.Application.Other;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Validators
{
    public class EquipmentValidator : AbstractValidator<EquipmentDto>
    {
        public EquipmentValidator()
        {
            RuleFor(x => x.EquipmentName)
                .NotEmpty().WithMessage("Наименование оборудования обязательно")
                .MaximumLength(50).WithMessage("Наименование не должно превышать 50 символов");
            RuleFor(x => x.HallId)
                .GreaterThan(0).WithMessage("Необходимо выбрать зал");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Количество должно быть больше нуля");
            RuleFor(x => x.InventoryNumber)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.InventoryNumber))
                .WithMessage("Инвернтарный номер не должен превышать 50 символов");
        }
    }
}
