using Microsoft.WindowsAzure.MobileServices;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Command;

namespace Votabl2.Models
{
    public class MainViewModel : ViewModel
    {
        public static MobileServiceClient Client;

        private static MainViewModel _instance;

        private IMobileServiceTable<Event> _eventsTable;

        static MainViewModel()
        {
            Client = new MobileServiceClient(
            "https://votabl2.azure-mobile.net/",
            "QYVwiXphqoLusPIjsPRFoGDxivzzIC27",
            new ActivityHandler());

            Client.SerializerSettings.CamelCasePropertyNames = true;
        }

        private MainViewModel()
        {
            _eventsTable = Client.GetTable<Event>();

            _insertCommand = new RelayCommand(Insert);
            _refreshCommand = new RelayCommand(Load);
            _chooseCommand = new RelayCommand<Event>(Choose);

            _newItem = new NewItemViewModel(Insert);

            BusyViewModel = BusyViewModel.Instance();
        }

        public Action<EventViewModel> NavigateAction { get; set; }

        public async void Login()
        {
            while (Client.CurrentUser == null)
            {
                try
                {
                    await Client.LoginAsync(MobileServiceAuthenticationProvider.Twitter, true);
                    Load();
                }
                catch (Exception) { }
            }
        }

        private void Choose(Event evt) 
        {
            EventViewModel mvm = new EventViewModel();
            mvm.Event = evt;
            NavigateAction(mvm);
        }

        private async void Insert()
        {
            var evt = new Event { Name = NewItem.Name, ImageUrl = "" };

            await _eventsTable.InsertAsync(evt);

            string readUrl = await BlobHelper.UploadImageToBlobStorage(NewItem.ImageFile, evt.ImageUrl);

            evt.ImageUrl = readUrl;

            Events.Add(evt);
        }

        private async void Load()
        {
            var events = await _eventsTable.ReadAsync();
            this.Events.Clear();
            this.Events.AddRange(events);
        }

        private readonly ObservableCollection<Event> _events = new ObservableCollection<Event>();
       
        public ObservableCollection<Event> Events
        {
            get { return _events; }
        }

        private string _newItemName;

        public string NewItemName
        {
            get { return _newItemName; }
            set
            {
                SetValue(ref _newItemName, value, "NewItemName");
            }
        }

        private RelayCommand<Event> _chooseCommand;

        public RelayCommand<Event> ChooseCommand
        {
            get { return _chooseCommand; }
            set
            {
                SetValue(ref _chooseCommand, value, "ChooseCommand");
            }
        }

        private RelayCommand _insertCommand;

        public RelayCommand InsertCommand
        {
            get { return _insertCommand; }
            set
            {
                SetValue(ref _insertCommand, value, "InsertCommand");
            }
        }

        private RelayCommand _refreshCommand;

        public RelayCommand RefreshCommand
        {
            get { return _refreshCommand; }
            set
            {
                SetValue(ref _refreshCommand, value, "RefreshCommand");
            }
        }

        private NewItemViewModel _newItem;

        public NewItemViewModel NewItem
        {
            get { return _newItem; }
            set
            {
                SetValue(ref _newItem, value, "NewItem");
            }
        }

        public static MainViewModel Single()
        {
            // assume use on UI thread
            if (_instance == null)
            {
                _instance = new MainViewModel();
            }
            return _instance;
        }

        public BusyViewModel BusyViewModel { get; private set; }

    }
}
