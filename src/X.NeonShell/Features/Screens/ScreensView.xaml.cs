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


            ////create new
            //var oh = new Screen()
            //{
            //    ScreenID = Guid.NewGuid(),
            //    Title = "test screen",
            //    DateStamp = DateTime.UtcNow,
            //};
            //var newid = oh.Save();

            var oh = new Screen(false);
            var count = oh.FindAll();
            if (count > 0) {
                tbCounter.Text = count.ToString();
            }

            ////load newly added
            //if (oh.Retrieve(newid))
            //{
            //    //delete newly added
            //    // oh.Delete(newid);
            //}

            //delete
            //oh.DeleteAll();

        }
    }



    public class Screen : SQLiteDataEntity
    {
        public string Title { get; set; }
        public DateTime DateStamp { get; set; }
        public Guid ScreenID { get; set; }

        public Screen(bool initDb = true) : base(initDb) { }
    }
}
