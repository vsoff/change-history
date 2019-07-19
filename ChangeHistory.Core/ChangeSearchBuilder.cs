using System;
using System.Collections;
using System.Collections.Generic;

namespace ChangeHistory.Core
{
    public class ChangeSearchBuilder<TModel>
    {
        private ChangesSearcher _changeController;
        private ICollection<SelectedProperty> _properties { get; }

        internal ChangeSearchBuilder(ChangesSearcher changeController)
        {
            _changeController = changeController;
            _properties = new List<SelectedProperty>();
        }

        public ChangeSearchBuilder<TModel> Select<TProp>(Func<TModel, TProp> func, int tag)
        {
            _properties.Add(new SelectedProperty(tag, x => func((TModel)x), typeof(TProp)));
            return this;
        }

        public void Build() => _changeController.SetChangeSearchSetting<TModel>(_properties);
    }
}
