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
        private IMobileServiceTable<Votabl> _votablsTable;

        public EventViewModel()
        {
            //TODO - uncomment
            //_votablsTable = MainViewModel.Client.GetTable<Votabl>();

            _loadVotesCommand = new RelayCommand(LoadVotes);
            _newItem = new NewItemViewModel(Create);
            
            var dataTransferManager = Windows.ApplicationModel.DataTransfer.DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;

            this.BusyViewModel = BusyViewModel.Instance();

            // TODO Register for RawVote message


        }

        void dataTransferManager_DataRequested(Windows.ApplicationModel.DataTransfer.DataTransferManager sender, Windows.ApplicationModel.DataTransfer.DataRequestedEventArgs args)
        {
            var message = string.Format("I need your opinion on {0}. Vote here: http://localhost:8080/index.html#/{1} #votabl2 #bldwin",
                Event.Name,
                Event.EventShareId);
            args.Request.Data.Properties.Title = "Share Votabl Link";
            args.Request.Data.Properties.Description = "Demonstrates how to share";
            args.Request.Data.SetText(message);
        }

        public async void Load()
        {
            // TODO - load votabls by eventShareId, clear and addrange

           
        }

        public async void LoadVotes()
        {
            // TODO - Load Votes

        }

        private async void Create()
        {
            var votabl = new Votabl { Name = NewItem.Name, ImageUrl = "", EventShareId = Event.EventShareId };

            // TODO - insert votabl, upload image and read back imageUrl


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
