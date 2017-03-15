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

namespace X.Desktop
{

    public sealed partial class MainPage : Page
    {
        public class OrderHeader : SQLiteDataEntity
        {
            public string Title { get; set; }
            public DateTime DateStamp { get; set; }
            public Guid OrderID { get; set; }
            public long Quantity { get; set; }
            public double TotalCost { get; set; }
        }


        public MainPage()
        {
            this.InitializeComponent();

            TestOrderHeaderModel();
        }

        public void TestOrderHeaderModel()
        {
            this.InitializeComponent();
            
            AppDatabase.Current.Init();
            
            //create new
            var oh = new OrderHeader();
            oh.OrderID = Guid.NewGuid();
            oh.Title = "test title";
            oh.DateStamp = DateTime.UtcNow;
            oh.Quantity = 100;
            oh.TotalCost = 199.00;
            var newid = oh.Save();

            //search
            var result = oh.Find("id = " + newid);

            //load 
            if (oh.Retrieve(newid))
            {
                //delete
                oh.Delete(newid);
            }

            //delete
            oh.DeleteAll();
        }
    }


  
}
