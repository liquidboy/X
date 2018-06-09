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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace X.ModernDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
  {
    public class OrderHeaderContext : SQLiteDataEntity<OrderHeader> { }
    public class OrderHeader : BaseEntity
    {
      public string Title { get; set; }
      public DateTime DateStamp { get; set; }
      public Guid OrderID { get; set; }
      public long Quantity { get; set; }
      public double TotalCost { get; set; }
    }

    public class OrderLiteItemContext : SQLiteDataEntity<OrderLiteItem> { }
    public class OrderLiteItem : BaseEntity
    {
      public Guid OrderID { get; set; }
      public long Quantity { get; set; }
      public string Product { get; set; }
      public double UnitCost { get; set; }
      public int OrderHeaderId { get; set; }
    }

    public class OrderFooterContext : SQLiteDataEntity<OrderFooter> { }
    public class OrderFooter : BaseEntity
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

      var ohctx = new OrderHeaderContext();
      var olictx = new OrderLiteItemContext();
      var ofctx = new OrderFooterContext();

      //create new
      var oh = new OrderHeader()
      {
        OrderID = Guid.NewGuid(),
        Title = "test title",
        DateStamp = DateTime.UtcNow,
        Quantity = 100,
        TotalCost = 199.00
      };
      var newid = ohctx.Save(oh);

      var oli = new OrderLiteItem()
      {
        Quantity = 1,
        Product = "XBox Scorpio",
        UnitCost = 499.00,
        OrderID = oh.OrderID,
        OrderHeaderId = newid
      };
      olictx.Save(oli);

      var of = new OrderFooter()
      {
        OrderID = oh.OrderID,
        PickupBy = "jose",
        ShippingAddress = "Home",
        OrderHeaderId = newid
      };
      ofctx.Save(of);


      //search
      var resultOrderHeader = ohctx.Find("id = " + newid);
      var resultOrderLineItems = olictx.Find("'OrderHeaderId' = " + newid);
      var resultOrderFooter = ofctx.Find("'ShippingAddress' = 'Home'");


      //load 
      if (ohctx.Retrieve(newid) != null)
      {
        //delete
        ohctx.Delete(newid);
      }

      //delete
      ohctx.DeleteAll();
    }
  }
}
