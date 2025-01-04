using DarkSky.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DarkSky.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, Type> viewModelsToViews = new();

        public void RegisterViewForViewModel(Type viewmodel, Type view)
        {
            viewModelsToViews[viewmodel] = view;
        }

        public void RegisterPaneForViewModel(Type viewmodel, Type view)
        {
            viewModelsToViews[viewmodel] = view;
        }

        public void NavigateTo<T>()
        {
            ((Frame)Window.Current.Content).Navigate(viewModelsToViews[typeof(T)]);
        }

        public void NavigatePaneTo<T>()
        {
            throw new NotImplementedException();
        }
    }
}
