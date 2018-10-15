using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
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
            //done in context constructor now via reflection
            //DBContext.Current.RegisterContext<OrderHeader>();
            //DBContext.Current.RegisterContext<OrderLiteItem>();
            //DBContext.Current.RegisterContext<OrderFooter>();
            
            //create new
            var oh = new OrderHeader()
            {
                OrderID = Guid.NewGuid(),
                Title = "test title",
                DateStamp = DateTime.UtcNow,
                Quantity = 100,
                TotalCost = 199.00
            };
            var newid = DBContext.Current.Save(oh);

            var oli = new OrderLiteItem()
            {
                Quantity = 1,
                Product = "XBox Scorpio",
                UnitCost = 499.00,
                OrderID = oh.OrderID,
                OrderHeaderId = newid
            };
            var newid2 = DBContext.Current.Save(oli);

            var of = new OrderFooter()
            {
                OrderID = oh.OrderID,
                PickupBy = "jose",
                ShippingAddress = "Home",
                OrderHeaderId = newid
            };
            var newid3 = DBContext.Current.Save(of);


            //search
            var resultOrderHeader = DBContext.Current.RetrieveEntity<OrderHeader>(oh.UniqueId);
            var olifound = DBContext.Current.RetrieveEntity<OrderLiteItem>(oli.UniqueId);
            //var resultOrderLineItems = Context.Current.Find<OrderLiteItem>($"Select * from OrderLite where 'OrderHeaderId' = ?", newid);
            var resultOrderFooter = DBContext.Current.RetrieveEntity<OrderFooter>(of.UniqueId);
            var resultOrderFooter2 = DBContext.Current.RetrieveEntities<OrderFooter>($"shippingaddress='Home'");
            

            //load 
            if (DBContext.Current.RetrieveEntity<OrderHeader>(oh.UniqueId) != null)
            {
                //delete
                DBContext.Current.Delete<OrderHeader>(newid);
            }

            //delete
            DBContext.Current.DeleteAll<OrderHeader>();

            //delete from manager
            DBContext.Current.Manager.DeleteAllDatabases();

        }
    }
}
