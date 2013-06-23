using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Votabl2.Common;
using Windows.Networking.PushNotifications;

namespace Votabl2.Models
{
    public class EventViewModel : ViewModel
    {
        private SynchronizationContext _context = SynchronizationContext.Current;
        private IMobileServiceTable<Votabl> _votablsTable;

        public EventViewModel()
        {
            _loadVotesCommand = new RelayCommand(LoadVotes);
            _newItem = new NewItemViewModel(Create);
            _votablsTable = MainViewModel.Client.GetTable<Votabl>();

            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;

            this.BusyViewModel = BusyViewModel.Instance();

            Messenger.Default.Register<RawVote>(this, vote =>
            {
                if (vote.EventShareId == Event.EventShareId)
                {
                    LoadVotes();
                }
            });
        }

        void dataTransferManager_DataRequested(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            var message = string.Format("Hey folks, I need your opinion on {0}. Vote here: http://localhost:8080/index.html#/{1} #votabl2 #bldwin",
                Event.Name,
                Event.EventShareId);
            args.Request.Data.Properties.Title = "Share Votabl Link";
            args.Request.Data.Properties.Description = "Demonstrates how to share";
            args.Request.Data.SetText(message);
        }

        public async void Load()
        {
            this.Details.Clear();

            var votabls = await _votablsTable.Where(v => v.EventShareId == Event.EventShareId).ToEnumerableAsync();

            this.Details.AddRange(votabls);
        }

        public async void LoadVotes()
        {
            var parameters = new Dictionary<string, string>() { { "eventShareId" , Event.EventShareId }};
            dynamic result = await MainViewModel.Client.InvokeApiAsync("eventCount", HttpMethod.Post, parameters); 
            IEnumerable<dynamic> arr = (IEnumerable<dynamic>) result;
            foreach (var votabl in this.Details)
            {
                var votes = arr.SingleOrDefault(c => c.votablId == votabl.Id);
                votabl.Count = votes == null ? 0 : votes.total;
            }
        }

        private async void Create()
        {
            var votabl = new Votabl { Name = NewItem.Name, ImageUrl = "", EventShareId = Event.EventShareId };

            await _votablsTable.InsertAsync(votabl);

            string readUrl = await BlobHelper.UploadImageToBlobStorage(NewItem.ImageFile, votabl.ImageUrl);

            votabl.ImageUrl = readUrl;

            this.Details.Add(votabl);
        }

        private Event _event;

        public Event Event
        {
            get { return _event; }
            set
            {
                SetValue(ref _event, value, "Event");
            }
        }

        private readonly ObservableCollection<Votabl> _details = new ObservableCollection<Votabl>();
        
        public ObservableCollection<Votabl> Details
        {
            get { return _details; }
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

        private RelayCommand _loadVotesCommand;

        public RelayCommand LoadVotesCommand
        {
            get { return _loadVotesCommand; }
            set
            {
                SetValue(ref _loadVotesCommand, value, "LoadVotesCommand");
            }
        }

        public BusyViewModel BusyViewModel { get; private set; }
    }
}
