using System;
using Windows.UI.Xaml.Controls;
using X.CoreLib.Shared.Framework.Services.DataEntity;
using X.NeonShell.Data;

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

            oh.Title = "updated title";
            newid = screenCtx.Save(oh);


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

}
