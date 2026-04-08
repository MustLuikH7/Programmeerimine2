using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<Users> DataSource { get; set; }
        Users SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        bool ConfirmDelete();

    }
}