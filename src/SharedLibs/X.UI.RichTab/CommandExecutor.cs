using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace X.UI.RichButton
{
    public class CommandExecuter
    {
        //http://nerobrain.blogspot.nl/2012/01/wpf-events-to-command.html


        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CommandExecuter), new PropertyMetadata(null, CommandPropertyChangedCallback));










        public static string GetOnEvent(DependencyObject obj)
        {
            return (string)obj.GetValue(OnEventProperty);
        }

        public static void SetOnEvent(DependencyObject obj, string value)
        {
            obj.SetValue(OnEventProperty, value);
        }
        
        public static readonly DependencyProperty OnEventProperty = DependencyProperty.RegisterAttached("OnEvent", typeof(string), typeof(CommandExecuter), new PropertyMetadata(null));











        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(CommandExecuter), new PropertyMetadata(null));










        //public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CommandExecuter), new PropertyMetadata(CommandPropertyChangedCallback));

        //public static readonly DependencyProperty OnEventProperty = DependencyProperty.RegisterAttached("OnEvent", typeof(string), typeof(CommandExecuter));

        //public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(CommandExecuter));

        public static void CommandPropertyChangedCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            string onEvent = (string)depObj.GetValue(OnEventProperty);
            //Debug.Assert(onEvent != null, "OnEvent must be set.");
            var eventInfo = depObj.GetType().GetEvent(onEvent);
            if (eventInfo != null)
            {
                var mInfo = typeof(CommandExecuter).GetMethod("OnRoutedEvent", BindingFlags.NonPublic | BindingFlags.Static);
                eventInfo.GetAddMethod().Invoke(depObj, new object[] { mInfo.CreateDelegate(eventInfo.EventHandlerType) });
            }
            else
            {
                //Debug.Fail(string.Format("{0} is not found on object {1}", onEvent, depObj.GetType()));

            }

        }
        //public static ICommand GetCommand(UIElement element)
        //{
        //    return (ICommand)element.GetValue(CommandProperty);
        //}
        //public static void SetCommand(UIElement element, ICommand command)
        //{
        //    element.SetValue(CommandProperty, command);
        //}
        //public static string GetOnEvent(UIElement element)
        //{
        //    return (string)element.GetValue(OnEventProperty);
        //}
        //public static void SetOnEvent(UIElement element, string evnt)
        //{
        //    element.SetValue(OnEventProperty, evnt);
        //}
        //public static object GetCommandParameter(UIElement element)
        //{
        //    return (object)element.GetValue(CommandParameterProperty);
        //}
        //public static void SetCommandParameter(UIElement element, object commandParam)
        //{
        //    element.SetValue(CommandParameterProperty, commandParam);
        //}
        private static void OnRoutedEvent(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            if (element != null)
            {
                ICommand command = element.GetValue(CommandProperty) as ICommand;
                if (command != null && command.CanExecute(element.GetValue(CommandParameterProperty)))
                {
                    command.Execute(element.GetValue(CommandParameterProperty));
                }
            }
        }
    }
}
