using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using mossotest.ViewModels;

namespace mossotest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsersListStatic : ContentPage
    {
        // set ViewModel for BindingContext
        UsersListStaticViewModel _viewModel
        {
            get { return BindingContext as UsersListStaticViewModel; }
            set { BindingContext = value; }
        }

        public UsersListStatic()
        {
            InitializeComponent();

            // setting BindingContext
            _viewModel = new UsersListStaticViewModel();

            _viewModel.LoadingStartedEvent += (sender, e) => { listView.IsRefreshing = true; };
            _viewModel.LoadingEndedEvent += (sender, e) => { listView.IsRefreshing = false; };

            ToolbarItems.Add(new ToolbarItem("Add", null, () => _viewModel.AddUserCommand.Execute(null)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // load data with API request
            _viewModel.LoadDataCommand.Execute(null);
        }

        // remove graphic effect on ListView
        void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView)sender).SelectedItem = null;
        }

        void SearchBar_Unfocused(object sender, FocusEventArgs e)
        {
            SearchBar searchBar = (sender as SearchBar);

            if (string.IsNullOrWhiteSpace(searchBar.Text))
            {
                _viewModel.RestoreUsersList();
                listView.IsPullToRefreshEnabled = true;
            }
        }

        void SearchBar_Focused(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchCommand.Execute((sender as SearchBar).Text);
            listView.IsPullToRefreshEnabled = false;
        }

        void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchCommand.Execute((sender as SearchBar).Text);
        }

    }
}