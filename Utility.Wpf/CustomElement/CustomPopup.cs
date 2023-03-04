using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Utility.Wpf.CustomElement
{
    public abstract class CustomPopupPlacementTarget
    {
        public abstract void AssignTo(Popup inPopup);

        public sealed class Mouse : CustomPopupPlacementTarget
        {
            public override void AssignTo(Popup inPopup)
                => inPopup.Placement = PlacementMode.Mouse;
        }

        public sealed class UIElement : CustomPopupPlacementTarget
        {
            public UIElement(PlacementMode inMode, System.Windows.UIElement inTargetElement)
            {
                TargetElement = inTargetElement;
                Mode = inMode;
            }

            public override void AssignTo(Popup inPopup)
            {
                inPopup.PlacementTarget = TargetElement!;
                inPopup.Placement = Mode;
            }

            public readonly System.Windows.UIElement TargetElement;
            public readonly PlacementMode Mode;
        }

        public sealed class Rectangle : CustomPopupPlacementTarget
        {
            public Rectangle(PlacementMode inMode, Rect inTargetRect)
            {
                Mode = inMode;
                TargetRect = inTargetRect;
            }

            public override void AssignTo(Popup inPopup)
            {
                inPopup.PlacementRectangle = TargetRect;
                inPopup.Placement = Mode;
            }

            public readonly Rect TargetRect;
            public readonly PlacementMode Mode;
        }
    }

    public class CustomPopupOpenArgs
    {
        public CustomPopupPlacementTarget Placement
        { get; set; } = new CustomPopupPlacementTarget.Mouse();

        public int? AutoHideTimeInMilliSeconds
        { get; set; } = null;
    }

    public abstract class CustomPopup
    {
        public CustomPopup()
        {
            Popup = new Popup
            {
                IsOpen = false,
                StaysOpen = false,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade,
                Focusable = true,
            };

            Popup.MouseDown += (_, _) => Popup.IsOpen = false;
        }

        public void Open(CustomPopupOpenArgs inArgs)
        {
            Popup.IsOpen = false;
            if (!BeforeOpen(inArgs))
                return;

            Popup.Child = GetPopupChild();

            inArgs.Placement.AssignTo(Popup);

            Popup.IsOpen = true;

            if (inArgs.AutoHideTimeInMilliSeconds.HasValue)
            {
                var timer = new DispatcherTimer {  Interval = TimeSpan.FromMilliseconds(inArgs.AutoHideTimeInMilliSeconds.Value) };
                timer.Tick += (_, _) =>
                {
                    Popup.IsOpen = false;
                    timer.Stop();
                };
                timer.Start();
            }
        }

        protected abstract UIElement GetPopupChild();
        protected virtual bool BeforeOpen(CustomPopupOpenArgs inArgs) { return true; }

        private readonly Popup Popup;
    }

    public class ErrorPopupOpenArgs : CustomPopupOpenArgs
    {
        public ErrorPopupOpenArgs(string inMessage)
        {
            Message = inMessage;
        }

        public readonly string Message;
    }

    public class ErrorPopup : CustomPopup
    {
        public ErrorPopup()
        {
            Border = new Border
            {
                Background = Brushes.Red,
                BorderBrush = Brushes.DarkRed,
                BorderThickness = new Thickness(1.0),
                Padding = new Thickness(4.0),
            };

            TextBlock = new TextBlock
            {
                Foreground = Brushes.White,
            };

            Border.Child = TextBlock;
        }

        protected override UIElement GetPopupChild()
            => Border;

        protected override bool BeforeOpen(CustomPopupOpenArgs inArgs)
        {
            if (!base.BeforeOpen(inArgs))
                return false;
            if (!(inArgs is ErrorPopupOpenArgs args))
                return false;

            TextBlock.Inlines.Clear();
            TextBlock.Inlines.Add(args.Message);

            return true;
        }

        private readonly Border Border;
        private readonly TextBlock TextBlock;
    }
}
