using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Utility.Wpf.CustomElement
{
    public class CustomDataGridTextColumn : DataGridTextColumn
    {
        public class ValidationCommandArgs
        {
            public ValidationCommandArgs(FrameworkElement inEditingElement)
            {
                EditingElement = inEditingElement;
                Text = EditingElement is TextBox textBox ? textBox.Text : "";
            }

            public readonly FrameworkElement EditingElement;
            public readonly string Text;
            public string? Error { get; set; } = null;

            public object DataContext => EditingElement.DataContext;
            public bool HasError => !string.IsNullOrEmpty(Error);
        }

        public ICommand? ValidationCommand
        {
            get => (ICommand)GetValue(ValidationCommandProperty);
            set => SetValue(ValidationCommandProperty, value);
        }

        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            var command = ValidationCommand;
            if (command != null)
            {
                var args = new ValidationCommandArgs(editingElement);
                if (command.CanExecute(args))
                {
                    command.Execute(args);
                    if (args.HasError)
                    {
                        new ErrorPopup().Open(
                            new ErrorPopupOpenArgs(args.Error!)
                            {
                                Placement = new CustomPopupPlacementTarget.Rectangle(
                                    PlacementMode.Bottom,
                                    editingElement.GetRectangleOnScreen()
                                ),
                                AutoHideTimeInMilliSeconds = 3000
                            }
                        );
                    }
                }
            }

            return base.CommitCellEdit(editingElement);
        }

        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            base.CancelCellEdit(editingElement, uneditedValue);
        }

        public static readonly DependencyProperty ValidationCommandProperty = DependencyProperty.RegisterAttached(
            "ValidationCommand", typeof(ICommand), typeof(CustomDataGridTextColumn), new PropertyMetadata(null)
        );
    }
}
