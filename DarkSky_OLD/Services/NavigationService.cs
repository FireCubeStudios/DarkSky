using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

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
