using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VittaClient.Tools
{
    public class NavigationService : INavigationService
    {
        private readonly Func<Type, object> _viewModelFactory;
        private readonly Dictionary<Type, Type> _viewModelToViewMapping;
        private Action<object> _setCurrentView;

        public NavigationService(Func<Type, object> viewModelFactory, Action<object> setCurrentView)
        {
            _viewModelFactory = viewModelFactory;
            _setCurrentView = setCurrentView;
            _viewModelToViewMapping = new Dictionary<Type, Type>();
        }

        public void RegisterViewMapping<TViewModel, TView>()
        {
            _viewModelToViewMapping[typeof(TViewModel)] = typeof(TView);
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            var viewModel = _viewModelFactory(typeof(TViewModel));
            if (_viewModelToViewMapping.TryGetValue(typeof(TViewModel), out var viewType))
            {
                var view = Activator.CreateInstance(viewType);
                if (view is FrameworkElement frameworkElement)
                {
                    frameworkElement.DataContext = viewModel;
                    _setCurrentView(frameworkElement);
                }
            }
        }
    }
}
