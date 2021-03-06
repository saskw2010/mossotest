using Rg.Plugins.Popup.Services;
using mossotest.Extensions;
using mossotest.Models;
using mossotest.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace mossotest.ViewModels
{
    public class UsersListStaticViewModel : BaseViewModel
    {
        #region Constants

        readonly static string ADMIN_ROLE = "ADMIN";
        readonly static string POPUP_DELETE_NOT_PERMITTED = "You cant't delete this user";
        readonly static string POPUP_SELFDELETE_MESSAGE = "You cant't delete yourself!";
        readonly static string POPUP_DELETE_MESSAGE = "Are you sure you want to delete this User?";

        #endregion

        #region Attributes and Properties

        // this collection main purpose is to store data from API request 
        ObservableCollection<User> _supportList;

        // this one instead is the ItemSource of the ListView. 
        // this allows to modify without any exceptions, the elements of the ListView.
        ObservableCollection<User> _usersList;

        public ObservableCollection<User> UsersList
        {
            get { return _usersList; }
            set { SetValue(ref _usersList, value); }
        }

        #endregion

        #region Commands

        public ICommand LoadDataCommand => new Command(async obj => await RefreshUsersList());

        public ICommand DeleteUserCommand => new Command<User>(async obj => await DeleteUserFromList(obj));

        public ICommand AddUserCommand => new Command(async obj => await AddNewUser());

        public ICommand SearchCommand => new Command<string>(obj => FilterUsersList(obj));

        #endregion


        async Task AddNewUser()
        {
            await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new RegisterNewUser());
        }

        async Task RefreshUsersList()
        {
            OnLoadingStarted(EventArgs.Empty);

            _supportList = await App.UserService.GETList();
            UsersList = _supportList;

            OnLoadingEnded(EventArgs.Empty);
        }

        public void RestoreUsersList()
        {
            UsersList = _supportList;
        }

        async Task DeleteUserFromList(User user) 
        {
            CustomAlertPopUp popUp = new CustomAlertPopUp();

            if (user.Id.Equals(Settings.UserId))
                popUp.Message = POPUP_SELFDELETE_MESSAGE;
            else if (user.Roles.Contains(ADMIN_ROLE))
                popUp.Message = POPUP_DELETE_NOT_PERMITTED;
            else
            {
                popUp.Message = POPUP_DELETE_MESSAGE;
                popUp.ButtonClickedEvent += async (sender, e) =>
                {
                    await App.UserService.DELETE(user.Id);
                    await RefreshUsersList();
                };
            }

            await PopupNavigation.Instance.PushAsync(popUp);
        }

        void FilterUsersList(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                // the filtering of elements is based on the elemnts Name. 
                // in case you wish to change, just replace el.Name with el.OtherField
                var tempRecords = _supportList.Where(el => el.Name.ToLower().Contains(query.ToLower()));
                UsersList = new ObservableCollection<User>(tempRecords);
            }
            else
                UsersList = new ObservableCollection<User>();

        }
    }
}