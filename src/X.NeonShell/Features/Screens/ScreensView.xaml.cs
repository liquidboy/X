using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.NeonShell.Features
{

    public sealed partial class ScreensView : Page
    {
        public ScreensView()
        {
            this.InitializeComponent();

            InitData();
        }


        private void InitData() {
            AppDatabase.Current.Init();
            //var screenCtx = ScreenContext.CreateReadOnly();
            var screenCtx = ScreenContext.Create();

            //create new
            var oh = new Screen()
            {
                ScreenID = Guid.NewGuid(),
                Title = "test screen",
                DateStamp = DateTime.UtcNow,
            };
            var newid = screenCtx.Save(oh);


            var count = screenCtx.FindAll();
            if (count > 0) {
                tbCounter.Text = count.ToString();
            }

            //load newly added
            if (screenCtx.Retrieve(newid) != null)
            {
                //delete newly added
                screenCtx.Delete(newid);
            }

            //delete
            screenCtx.DeleteAll();

        }
    }

    public class Screen : BaseEntity {
        public string Title { get; set; }
        public DateTime DateStamp { get; set; }
        public Guid ScreenID { get; set; }

    }

    public class ScreenContext : SQLiteDataEntity<Screen>
    {

    }
}
