using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votabl2.Common
{
    public class Disposer : IDisposable
    {
        private bool _disposed = false;
        private Action _onDisposal;

        public Disposer(Action onDisposal)
        {
            _onDisposal = onDisposal;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _onDisposal();
            }
        }
    }

    public class BusyDisposer : Disposer
    {
        public BusyDisposer() : base(() => Messenger.Default.Send<bool>(false, "busy"))
        {
            Messenger.Default.Send<bool>(true, "busy");
        }
    }
}
