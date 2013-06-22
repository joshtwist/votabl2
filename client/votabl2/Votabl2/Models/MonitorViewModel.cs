using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;

namespace Votabl2.Models
{
    public class MonitorViewModel : ViewModel
    {
        private IMobileServiceTable<Votabl> _votablsTable;

        public MonitorViewModel()
        {
            _newItem = new NewItemViewModel(Create);
            _votablsTable = MainViewModel.Client.GetTable<Votabl>();
        }

        public async void Load()
        {
            //dynamic j = new JObject();
            //j.eventId = Event.Id;
            //var result = await MainViewModel.Client.InvokeApiAsync("monitorEvent", (JObject)j);
            //foreach (dynamic item in result)
            //{
            //    _details.Add(item);
            //}

            this.Details.Clear();

            var votabls = await _votablsTable.Where(v => v.EventId == Event.Id).ToEnumerableAsync();

            this.Details.AddRange(votabls);
        }

        private async void Create()
        {
            var votabl = new Votabl { Name = NewItem.Name, ImageUrl = "", EventId = Event.Id };

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

        private readonly ObservableCollection<dynamic> _details = new ObservableCollection<dynamic>();

        public ObservableCollection<dynamic> Details
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


    }
}
