using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Application.Interfaces
{
    public interface IDialogService
    {
        bool ShowConfirmation(string message, string title);
        void ShowError(string message, string title);
        void ShowInformation(string message, string title);
    }
}
