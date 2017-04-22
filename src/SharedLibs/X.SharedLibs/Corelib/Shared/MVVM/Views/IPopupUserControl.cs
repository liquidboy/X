using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavouriteMX.Shared.Views.Controls
{
    public interface IPopupUserControl
    {
        void LoadControl(string aggregateId);
        void UnloadControl();
        bool EditingIsDisabled { get; set; }
    }
}
