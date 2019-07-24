using System;
using System.Collections;
using System.Collections.Generic;

namespace ChangeHistory.Core.Changes
{
    public class ChangeSearchBuilder<TModel> : IChangeSearchBuilder<TModel>
    {
        private ChangesSearcher _changeController;
        private ICollection<SelectedProperty> _properties { get; }

        internal ChangeSearchBuilder(ChangesSearcher changeController)
        {
            _changeController = changeController;
            _properties = new List<SelectedProperty>();
        }

        public ChangeSearchBuilder<TModel> Select(int tag, string propertyName)
        {
            _properties.Add(new SelectedProperty(typeof(TModel).GetProperty(propertyName), tag));
            return this;
        }

        public void Build() => _changeController.SetChangeSearchSetting<TModel>(_properties);
    }
}
