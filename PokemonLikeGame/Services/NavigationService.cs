using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PokemonLikeGame.Services
{
    public class NavigationService
    {
        private readonly Dictionary<string, UserControl> _views = new Dictionary<string, UserControl>();
        private readonly ContentControl _contentControl;

        public NavigationService(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public void Register(string key, UserControl view)
        {
            if (!_views.ContainsKey(key))
            {
                _views.Add(key, view);
            }
        }

        public void Navigate(string key)
        {
            if (_views.ContainsKey(key))
            {
                _contentControl.Content = _views[key];
            }
        }
    }
}
