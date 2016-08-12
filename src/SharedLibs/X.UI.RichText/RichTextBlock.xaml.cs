using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace X.UI.RichText
{
    public sealed partial class RichTextBlock : UserControl
    {

        static SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
        static SolidColorBrush orangeBrush = new SolidColorBrush(Colors.Orange);
        static SolidColorBrush blueBrush = new SolidColorBrush(Colors.Blue);


        public RichTextBlock()
        {
            this.InitializeComponent();
        }





        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RichTextBlock), new PropertyMetadata(string.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rtc = sender as RichTextBlock;
            var control = rtc.FindName("rtbMain") as Windows.UI.Xaml.Controls.RichTextBlock;
            //var control = sender as RichTextBlock;
            if (control != null)
            {
                control.Blocks.Clear();
                string value = e.NewValue.ToString();

                var paragraph = ParseForRichTextParagraph(value);
                control.Blocks.Add(paragraph);
            }

        }

        private static Paragraph ParseForRichTextParagraph(string message)
        {
            var ret = new Paragraph();

            var words = message.Split(" ".ToCharArray());
            foreach (var word in words)
            {
                if (word.Contains("@"))
                    ret.Inlines.Add(new Run { Text = word + " ", Foreground = greenBrush });
                else if (word.Contains("#"))
                    ret.Inlines.Add(new Run { Text = word + " ", Foreground = orangeBrush });
                else if (word.Contains("http"))
                {
                    var ul = new Underline();
                    ul.Inlines.Add(new Run { Text = word + " ", Foreground = blueBrush });
                    ret.Inlines.Add(ul);
                }
                else
                    ret.Inlines.Add(new Run { Text = word + " " });
            }

            return ret;
        }
    }
}
