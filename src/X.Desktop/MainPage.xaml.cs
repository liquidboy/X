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

        public class OrderLiteItem : SQLiteDataEntity 
        {
            public Guid OrderID { get; set; }
            public long Quantity { get; set; }
            public string Product { get; set; }
            public double UnitCost  { get; set; }
            public int OrderHeaderId { get; set; }
        }

        public class OrderFooter : SQLiteDataEntity
        {
            public Guid OrderID { get; set; }
            public string ShippingAddress { get; set; }
            public string PickupBy { get; set; }
            public int OrderHeaderId { get; set; }
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
            var oh = new OrderHeader() { OrderID = Guid.NewGuid(), Title = "test title",
                DateStamp = DateTime.UtcNow, Quantity = 100, TotalCost = 199.00 };
            var newid = oh.Save();

            var oli = new OrderLiteItem() { Quantity = 1, Product = "XBox Scorpio", UnitCost = 499.00,
                OrderID = oh.OrderID, OrderHeaderId = newid };
            oli.Save();

            var of = new OrderFooter() { OrderID = oh.OrderID, PickupBy = "jose",
                ShippingAddress = "Home", OrderHeaderId = newid };
            of.Save();


            //search
            var resultOrderHeader = oh.Find("id = " + newid);
            var resultOrderLineItems = oli.Find("'OrderHeaderId' = " + newid );
            var resultOrderFooter = of.Find("'ShippingAddress' = 'Home'" );


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
