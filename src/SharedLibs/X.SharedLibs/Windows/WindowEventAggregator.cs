﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace X.SharedLibs.Windows
{
  public interface ISubscriber<in TMessage>
  {
    void HandleMessage(TMessage message);
  }

  public class WindowEventAggregator
  {
    private static List<Tuple<CoreDispatcher, object>> subscribers = new List<Tuple<CoreDispatcher, object>>();

    public void Subscribe<TMessage>(ISubscriber<TMessage> subscriber)
    {
      subscribers.Add(new Tuple<CoreDispatcher, object>(Window.Current.Dispatcher, subscriber));
    }

    public async void Publish<TMessage>(TMessage message)
    {
      var messageType = GetEventType(message);

      foreach (var subscriber in subscribers)
      {
        var handler = subscriber.Item2;

        if (messageType.IsInstanceOfType(handler))
        {
          var dispatcher = subscriber.Item1;
          await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
          {
            ((ISubscriber<TMessage>)handler).HandleMessage(message);
          });
        }
      }
    }

    private static Type GetEventType<T>(T args)
    {
      return typeof(ISubscriber<>).MakeGenericType(args.GetType());
    }

  }
}
