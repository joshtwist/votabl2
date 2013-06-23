using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;

namespace Votabl2.Models
{
    public class EventViewModel : ViewModel
    {
        private IMobileServiceTable<Votabl> _votablsTable;

        public EventViewModel()
        {
            _loadVotesCommand = new DelegateCommand(LoadVotes);
            _newItem = new NewItemViewModel(Create);
            _votablsTable = MainViewModel.Client.GetTable<Votabl>();
            votabl2Push.NotificationArrived = s =>
            {
                LoadVotes();
            };
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

        private DelegateCommand _loadVotesCommand;

        public DelegateCommand LoadVotesCommand
        {
            get { return _loadVotesCommand; }
            set
            {
                SetValue(ref _loadVotesCommand, value, "LoadVotesCommand");
            }
        }


    }
}
