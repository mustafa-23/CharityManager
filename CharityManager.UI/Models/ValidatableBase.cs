using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace CharityManager.UI.Models
{
    public class ValidatableBase:ModelBase,INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        protected new bool SetProperty<T>(Expression<Func<T>> expression, T value)
        {
            ValidateProperty(((MemberExpression)expression.Body).Member.Name, value);
            return base.SetProperty<T>(expression, value);
        }

        public void ValidateProperty<T>(string propertyName, T val)
        {
            var results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(this)
            {
                MemberName = propertyName,
            };
            Validator.TryValidateProperty(val, context, results);

            if (results.Any())
                _errors[propertyName] = results.Select(c => c.ErrorMessage.ToString()).ToList();
            else
                _errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #region INotifyDataErrorInfo
        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName != null && _errors.ContainsKey(propertyName))
                return _errors[propertyName];
            return null;
        }
        public string GetErrorString()
        {
            string errorString = string.Empty;
            foreach (var item in _errors)
                errorString += string.Join(",", item.Value) + "\n";
            return errorString;
        }
        #endregion

        public virtual bool Validate()
        {
            return true;
        }
    }
}
