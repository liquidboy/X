using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using X.NeonShell.Features;

namespace X.NeonShell
{
    public partial class MainPage : Page
    {
        // http://modernicons.io/segoe-mdl2/cheatsheet/
        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario(){
                Title = "Home",
                Icon = System.Net.WebUtility.HtmlDecode("&#xE10F;"),
                Description = "",
                ClassType=typeof(PublicDashboardView)
            },
           new Scenario() {
                Title = "Your Account",
                Icon = System.Net.WebUtility.HtmlDecode("&#xE1CE;"),
                Description = "",
                ClassType=typeof(YourAccountView)
            },
            new Scenario() {
                Title = "Your Photos",
                Icon= System.Net.WebUtility.HtmlDecode("&#xE11B;"),
                Description = "",
                ClassType=typeof(YourDashboardView)
            },
            new Scenario() {
                Title = "Screens",
                Icon= System.Net.WebUtility.HtmlDecode("&#xE11B;"),
                Description = "",
                ClassType=typeof(ScreensView)
            }
        };

        public List<Scenario> Scenarios
        {
            get { return this.scenarios; }
        }

    }

    public class Scenario
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public Type ClassType { get; set; }
    }
}
