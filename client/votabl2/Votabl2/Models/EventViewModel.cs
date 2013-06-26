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
        public EventViewModel()
        {
            //_votablsTable = MainViewModel.Client.GetTable<Votabl>();

            
            // TODO Register for RawVote message


            Setup();
        }


        public async void Load()
        {
            // TODO - load votabls by eventShareId, clear and addrange
            //var votabls = await _votablsTable.
            //    Where(v => v.EventShareId == Event.EventShareId).
            //    ToEnumerableAsync();

            //this.Details.Clear();
            //this.Details.AddRange(votabls);
        }

        private async void Insert()
        {
            var votabl = new Votabl { Name = NewItem.Name, EventShareId = Event.EventShareId };

            // TODO - insert votabl, upload image and read back imageUrl
            //await _votablsTable.InsertAsync(votabl);

            //string readUrl = await BlobHelper.UploadImageToBlobStorage(NewItem.ImageFile, votabl.ImageUrl);
            //votabl.ImageUrl = readUrl;

            this.Details.Add(votabl);
        }

        public async void LoadVotes()
        {
            // TODO - Load Votes

        }

        #region boring stuff

        private IMobileServiceTable<Votabl> _votablsTable;

        private void Setup()
        {
            _loadVotesCommand = new RelayCommand(LoadVotes);
            _newItem = new NewItemViewModel(Insert);

            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;

            this.BusyViewModel = BusyViewModel.Instance();
        }

        void dataTransferManager_DataRequested(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            var message = string.Format("I need your opinion on {0} - http://www.votabl.net/votabl2/index.html#/{1} #votabl2 #bldwin",
                Event.Name,
                Event.EventShareId);
            args.Request.Data.Properties.Title = "Share Votabl Link";
            args.Request.Data.Properties.Description = "Demonstrates how to share";
            args.Request.Data.SetText(message);
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

#endregion
    }
}
