using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using DarkSky.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DarkSky.Core.Cursors
{
    /*
	 * Implements ICursorSource with common logic for refreshing and a constructor with ATProtocol service
	 * https://docs.bsky.app/docs/tutorials/viewing-feeds
	 */
    public abstract partial class AbstractCursorSource<T> : ObservableObject, ICursorSource
    {
        [ObservableProperty]
        private bool isLoading = false;

        public IEnumerable Items { get; } = new ObservableCollection<T>();

        protected string Cursor = "";
        protected ATProtoService atProtoService = ServiceContainer.Services.GetService<ATProtoService>();

        public virtual async Task GetMoreItemsAsync(int limit = 20)
        {
            try
            {
                if (IsLoading) return; // Don't load if items are currently loading
                IsLoading = true;

                // Call the inner implementation method for inheriting classes to load items
                // GetMoreItemsAsync() is virtual so specialised classes can override even this logic
                await OnGetMoreItemsAsync(limit);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new ErrorMessage(ex));
            }
        }

        protected abstract Task OnGetMoreItemsAsync(int limit = 20);

        public async Task RefreshAsync()
        {
            Clear();
            await GetMoreItemsAsync();
        }

        public virtual void Clear()
        {
            ((ObservableCollection<T>)Items).Clear();
            Cursor = "";
        }

        protected void Add(T item) => ((ObservableCollection<T>)Items).Add(item);
    }
}
