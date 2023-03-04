﻿using System.Windows;

namespace Utility.Wpf
{
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
            => new BindingProxy();

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(object),typeof(BindingProxy), new UIPropertyMetadata(null)
        );
    }
}
