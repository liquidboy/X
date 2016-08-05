using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.SL.Core
{
    public class EventObject : IDisposable
    {
        enum Flags
        {
            MultiThreadedSafe = 1 << 28, // if the dtor can be called on any thread
            Attached = 1 << 29,
            Disposing = 1 << 30,
            Disposed = 1 << 31,
            IdMask = ~(Attached | Disposed | Disposing | MultiThreadedSafe),
        };
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
