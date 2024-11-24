using DarkSky.Services;
using DarkSky.ViewModels;
using DarkSky.Views;
using DarkSky.Services;
using FishyFlip;
using FishyFlip.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DarkSky
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
		private static IServiceProvider ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddSingleton<ICredentialService, CredentialService>();
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<ATProtoService>();
			services.AddTransient<LoginViewModel>();
			services.AddTransient<MainViewModel>();
            services.AddTransient<ProfileViewModel>();
			services.AddTransient<HomeFeedViewModel>();

			NavigationService navigationService = new();
			navigationService.RegisterViewForViewModel(typeof(MainViewModel), typeof(MainPage));
			navigationService.RegisterViewForViewModel(typeof(LoginViewModel), typeof(LoginPage));
			services.AddSingleton<INavigationService>(navigationService);

			return services.BuildServiceProvider();
		}
		/// <summary>
		/// Gets the current <see cref="App"/> instance in use
		/// </summary>
		public new static App Current => (App)Application.Current;

		/// <summary>
		/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
		/// </summary>
		public IServiceProvider Services { get; }

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
        {
			Services = ConfigureServices();
			this.InitializeComponent();
			this.Suspending += OnSuspending;
			UnhandledException += OnUnhandledException;
			TaskScheduler.UnobservedTaskException += OnUnobservedException;
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
					// ASSUMES NOT LOGGED IN DUE TO NO CREDENTIALS STORED
					var a = App.Current.Services.GetService<ATProtoService>();
	            }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

		private static void OnUnobservedException(object? sender, UnobservedTaskExceptionEventArgs e) => e.SetObserved();

		private static async void OnUnhandledException(object? sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
		{
			await new ContentDialog // This code was taken directly from the UnifiedApp class. When a UWP-targeting version of it is created, it will be used here.
				{
					Title = "Unhandled exception",
					Content = e.Message,
					CloseButtonText = "Close"
				}
				.ShowAsync();
        }

		private void CurrentDomain_FirstChanceException(object? sender, FirstChanceExceptionEventArgs e)
		{
		}
	}
}
