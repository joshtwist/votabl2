using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;

namespace Votabl2.Models
{
    public class BusyViewModel : ViewModel
    {
        private static BusyViewModel _instance;

        private int _activityCount = 0;

        public BusyViewModel()
        {
            Messenger.Default.Register<bool>(this, "busy", busy =>
            {
                if (busy)
                {
                    Busy();
                }
                else
                {
                    Idle();
                }
            });
        }

        public void Busy()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                _activityCount++;
                if (_activityCount == 1)
                {
                    IsBusy = true;
                }
            });
        }

        public void Idle()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                _activityCount--;
                if (_activityCount == 0)
                {
                    IsBusy = false;
                }
            });
        }

        

        public static BusyViewModel Instance()
        {
            if (_instance == null)
            {
                _instance = new BusyViewModel();
            }
            return _instance;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetValue(ref _isBusy, value, "IsBusy");
            }
        }
    }
}
