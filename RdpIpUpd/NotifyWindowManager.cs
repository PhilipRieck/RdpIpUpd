using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RdpIpUpd
{
    public class NotifyWindowManager : WindowManager
    {
        protected override System.Windows.Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            if(! (rootModel is NotifyViewModel))
            {
                return base.CreateWindow(rootModel, isDialog, context, settings);
            }
            return null;
        }
    }
}
