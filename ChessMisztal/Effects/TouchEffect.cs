using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChessMisztal.Effects
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public ICommand Command { get; set; }

        public TouchEffect() : base("ChessMisztal.Effects.TouchEffect")
        {
        }

    public bool Capture { set; get; }

        public void OnTouchAction(VisualElement element, TouchActionEventArgs args)
        {
            Command?.Execute(args);
            TouchAction?.Invoke(element, args);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(TouchEffect), null, propertyChanged: OnCommandChanged);
        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }
        public static void SetCommand(BindableObject view, ICommand command)
        {
            view.SetValue(CommandProperty, command);
        }

        private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // When the 'Command' property changes, set the native control's command
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(TouchEffect), null);
        public static object GetCommandParameter(BindableObject view)
        {
            return view.GetValue(CommandParameterProperty);
        }
        public static void SetCommandParameter(BindableObject view, object parameter)
        {
            view.SetValue(CommandParameterProperty, parameter);
        }
    }

    public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(float locationX, float locationY, TouchActionType type, bool isInContact)
        {
            LocationX = locationX;
            LocationY = locationY;
            Type = type;
            IsInContact = isInContact;
        }

        public float LocationX { private set; get; }

        public float LocationY { private set; get; }

        public TouchActionType Type { private set; get; }

        public bool IsInContact { private set; get; }
    }

    public enum TouchActionType
    {
        Entered,
        Pressed,
        Moved,
        Released,
        Exited,
        Cancelled
    }
}