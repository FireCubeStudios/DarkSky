using DarkSky.Core.ViewModels.Temporary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DarkSky.Templates
{
    public class CursorItemSelector : DataTemplateSelector
    {
        public DataTemplate PostItemTemplate { get; set; }
        public DataTemplate ProfileItemTemplate { get; set; }
        public DataTemplate ListItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is PostViewModel)
            {
                return PostItemTemplate;
            }
            else if (item is ProfileViewModel)
            {
                return ProfileItemTemplate;
            }
            else if (item is ListViewModel)
            {
                return ListItemTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
