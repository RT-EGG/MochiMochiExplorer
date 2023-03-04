using Reactive.Bindings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public interface IValidationError
    {
        string Key { get; }
        string Message { get; }
    }

    class ValidationError : IValidationError
    {
        public ValidationError(string inKey)
            : this(inKey, string.Empty)
        { }

        public ValidationError(string inKey, string inMessage)
        {
            Key = inKey;
            Message.Value = inMessage;
        }

        public string Key { get; }
        string IValidationError.Message => Message.Value;
        public ReactiveProperty<string> Message { get; } = new ReactiveProperty<string>(string.Empty);

    }

    public class ValidationErrorChangedEventArgs : EventArgs
    {
        public ValidationErrorChangedEventArgs(string inKey, string inMessage)
        {
            Key = inKey;
            Message = inMessage;
        }

        public readonly string Key;
        public readonly string Message;
    }
    public delegate void ValidationErrorChangedEvent(ValidationErrorChangedEventArgs args);

    public class ValidationErrorList : IEnumerable<IValidationError>
    {
        public void AddOrSet(string inKey, string inMessage)
        {
            if (!TryGetError(inKey, out ValidationError? error))
            {
                error = new ValidationError(inKey, inMessage);
                error.Message.Subscribe(message =>
                {
                    var args = new ValidationErrorChangedEventArgs(inKey, message);
                    ValidationErrorChanged?.Invoke(args);
                });

                _items.Add(error);
            }
            else
            {
                error!.Message.Value = inMessage;
            }
        }

        public bool TryGetError(string inKey, out string? outMessage)
        {
            if (TryGetError(inKey, out ValidationError? outError))
            {
                outMessage = outError!.Message.Value;
                return true;
            }
            outMessage = null;
            return false;
        }

        public bool HasAnyError => _items.Any(item => !string.IsNullOrEmpty(item.Message.Value));

        public event ValidationErrorChangedEvent? ValidationErrorChanged;

        private bool TryGetError(string inKey, out ValidationError? outError)
        {
            outError = _items.FirstOrDefault(item => item.Key == inKey);
            return outError is not null;
        }

        IEnumerator<IValidationError> IEnumerable<IValidationError>.GetEnumerator()
            => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => _items.GetEnumerator();

        private ReactiveCollection<ValidationError> _items = new ReactiveCollection<ValidationError>();
    }
}
