using SumoNinjaMonkey.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.CoreLib.Shared.Framework.Services.DataEntity;

namespace X.ModernDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SQLitePage : Page
    {
        public class OrderHeader : BaseEntity
        {
            public string Title { get; set; }
            public DateTime DateStamp { get; set; }
            public Guid OrderID { get; set; }
            public long Quantity { get; set; }
            public double TotalCost { get; set; }
        }

        public class OrderLiteItem : BaseEntity
        {
            public Guid OrderID { get; set; }
            public long Quantity { get; set; }
            public string Product { get; set; }
            public double UnitCost { get; set; }
            public int OrderHeaderId { get; set; }
        }

        public class OrderFooter : BaseEntity
        {
            public Guid OrderID { get; set; }
            public string ShippingAddress { get; set; }
            public string PickupBy { get; set; }
            public int OrderHeaderId { get; set; }
        }


        public SQLitePage()
        {
            this.InitializeComponent();

            TestOrderHeaderModel();
            TestDynamicUI();
            InitChrome();
        }

        private void InitChrome()
        {
            ctlHeader.InitChrome(App.Current, ApplicationView.GetForCurrentView());
        }

        private void TestDynamicUI()
        {

        }

        public void TestOrderHeaderModel()
        {
            this.InitializeComponent();

            AppDatabase.Current.Init();
            Context.Current.RegisterContext<OrderHeader>();
            Context.Current.RegisterContext<OrderLiteItem>();
            Context.Current.RegisterContext<OrderFooter>();
            
            //create new
            var oh = new OrderHeader()
            {
                OrderID = Guid.NewGuid(),
                Title = "test title",
                DateStamp = DateTime.UtcNow,
                Quantity = 100,
                TotalCost = 199.00
            };
            var newid = Context.Current.Save(oh);

            var oli = new OrderLiteItem()
            {
                Quantity = 1,
                Product = "XBox Scorpio",
                UnitCost = 499.00,
                OrderID = oh.OrderID,
                OrderHeaderId = newid
            };
            var newid2 = Context.Current.Save(oli);

            var of = new OrderFooter()
            {
                OrderID = oh.OrderID,
                PickupBy = "jose",
                ShippingAddress = "Home",
                OrderHeaderId = newid
            };
            var newid3 = Context.Current.Save(of);


            //search
            var resultOrderHeader = Context.Current.RetrieveEntity<OrderHeader>(oh.UniqueId);
            var olifound = Context.Current.RetrieveEntity<OrderLiteItem>(oli.UniqueId);
            //var resultOrderLineItems = Context.Current.Find<OrderLiteItem>($"Select * from OrderLite where 'OrderHeaderId' = ?", newid);
            var resultOrderFooter = Context.Current.RetrieveEntity<OrderFooter>(of.UniqueId);
            var resultOrderFooter2 = Context.Current.RetrieveEntity<OrderFooter>($"shippingaddress='Home'");


            //load 
            if (Context.Current.RetrieveEntity<OrderHeader>(oh.UniqueId) != null)
            {
                //delete
                Context.Current.Delete<OrderHeader>(newid);
            }

            //delete
            Context.Current.DeleteAll<OrderHeader>();

            //delete from manager
            SqliteDatabaseManager.Current.DeleteAllDatabases();

        }
    }
}
