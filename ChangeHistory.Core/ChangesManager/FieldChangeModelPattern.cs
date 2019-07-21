using System;
using System.Reflection;

namespace ChangeHistory.Core.ChangesManager
{
    public class FieldChangeModelPattern<TModel> where TModel : class
    {
        public string Header { get; set; }
        public Func<object, string> FormatFunc { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}
