using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using mossotest.ViewModels;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mossotest.Extensions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePasswordPopUp : PopupPage
	{
        ChangePasswordPopUpViewModel _viewModel
        {
            get { return BindingContext as ChangePasswordPopUpViewModel; }
            set { BindingContext = value; }
        }

		public ChangePasswordPopUp()
		{
            InitializeComponent();

            _viewModel = new ChangePasswordPopUpViewModel();

            _viewModel.LoadingStartedEvent += (sender, e) => { loading_view.IsVisible = true; };
            _viewModel.LoadingEndedEvent += (sender, e) => { loading_view.IsVisible = false; };
        }

        void Dismiss_PopUp_Handler(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        async Task Change_Password_Handler(object sender, System.EventArgs e)
        {
            string oldPassword = entry_old_password.Text;
            string newPassword = entry_new_password.Text;
            string confirmedPassword = entry_confirm_new_password.Text;

            // check if any field is empty
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmedPassword))
                label_message.Text = "One or more fields are empty";

            // check if OldPassword is equals to UserPassword
            else if (!oldPassword.Equals(Settings.Password))
                label_message.Text = "Old password is not correct";

            // check if NewPassword and ConfirmedPassword are equals
            else if (!newPassword.Equals(confirmedPassword))
                label_message.Text = "Passwords do not match";

            // if old password and new password are identical
            else if (newPassword.Equals(oldPassword) && newPassword.Equals(confirmedPassword))
                label_message.Text = "Old password and new password are identical";

            // if everything is correct
            else {
                // try to save
                if (await _viewModel.ChangePassword(oldPassword, newPassword))
                {
                    label_message.Text = "Password successfully changed";
                }
                else
                    label_message.Text = "An error has occured, please try again";
            }
        }
    }
}