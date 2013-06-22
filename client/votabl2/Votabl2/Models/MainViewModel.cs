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

namespace Votabl2.Models
{
    public class MainViewModel : ViewModel
    {
        public static MobileServiceClient Client = new MobileServiceClient(
            "https://votabl2.azure-mobile.net/",
            "flEkqDIimXoiDPxXqwPzFSBwBbFJSg55");

        private static MainViewModel _instance;

        private IMobileServiceTable<Event> _eventsTable;
        private IMobileServiceTable<Votabl> _votablsTable;
        private IMobileServiceTable<Vote> _votesTable;

        private StorageFile _newImageFile;

        private MainViewModel()
        {
            _eventsTable = Client.GetTable<Event>();
            _votablsTable = Client.GetTable<Votabl>();
            _votesTable = Client.GetTable<Vote>();

            _insertCommand = new DelegateCommand(Insert);
            _refreshCommand = new DelegateCommand(Load);
            _chooseCommand = new DelegateCommand<Event>(Choose);

            _newItem = new NewItemViewModel(Insert);
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

        private DelegateCommand<Event> _chooseCommand;

        public DelegateCommand<Event> ChooseCommand
        {
            get { return _chooseCommand; }
            set
            {
                SetValue(ref _chooseCommand, value, "ChooseCommand");
            }
        }

        private DelegateCommand _insertCommand;

        public DelegateCommand InsertCommand
        {
            get { return _insertCommand; }
            set
            {
                SetValue(ref _insertCommand, value, "InsertCommand");
            }
        }

        private DelegateCommand _refreshCommand;

        public DelegateCommand RefreshCommand
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
    }
}
