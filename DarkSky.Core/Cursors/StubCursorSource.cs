using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
	public partial class StubCursorSource : ObservableObject, ICursorSource
	{
		[ObservableProperty]
		private bool isLoading = false;

		public IEnumerable Items { get; } = new ObservableCollection<object>();

		public void Clear()
		{

		}

		public Task GetMoreItemsAsync(int limit = 50)
		{
			return Task.CompletedTask;
		}

		public Task RefreshAsync()
		{
			return Task.CompletedTask;
		}
	}
}
