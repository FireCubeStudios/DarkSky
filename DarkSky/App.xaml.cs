using DarkSky.Core.Classes;
using DarkSky.Core.Services;
using DarkSky.Core.Services.Interfaces;
using DarkSky.Core.ViewModels;
using DarkSky.Services;
using Microsoft.Extensions.DependencyInjection;
using Sentry;
using Sentry.Protocol;
using System;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DarkSky
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ICredentialService, CredentialService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IKeyService, KeyService>();
            services.AddSingleton<ATProtoService>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<NotificationsViewModel>();
            services.AddSingleton<HomeFeedViewModel>();
            services.AddSingleton<ListsViewModel>();

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
        public IServiceProvider Services { get; set; }

        public ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

        private CredentialService CredentialService = new CredentialService();

        private bool loginfail = false;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            App.Current.Services = ServiceContainer.Services = ConfigureServices();
            ATProtoSetup();

            Suspending += OnSuspending;
            UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            Helpers.Sentry.Init(); // Initialize Sentry SDK
        }

        private async void ATProtoSetup()
        {

            if (CredentialService.Count() != 0)
            {
                try
                {
                    Credential credentials = CredentialService.GetCredential();
                    ATProtoService proto = Services.GetService<ATProtoService>();

                    if (String.IsNullOrEmpty((string)Settings.Values["v1_previous_did"])) // legacy, login normal way then save new details
                    {
                        await proto.LoginAsync(credentials.username, credentials.password);
                    }
                    else
                    { // login with refresh token if it is available
                        try
                        {
                            await proto.RefreshLoginAsync(
                                (string)Settings.Values["v1_previous_did"],
                                (string)Settings.Values["v1_previous_handle"],
                                (string)Settings.Values["v1_previous_accessJWT"],
                                credentials.token,
                                (string)Settings.Values["v1_previous_pds"]
                                );
                        }
                        catch (Exception e)
                        {
                            await proto.LoginAsync(credentials.username, credentials.password);
                        }
                    }
                }
                catch (Exception e)
                {
                    loginfail = true;
                }
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
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
                Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);
                if (rootFrame.Content == null)
                {
                    if (CredentialService.Count() == 0 || loginfail)
                    {
                        rootFrame.Navigate(typeof(NEWLoginPage), e.Arguments);
                    }
                    else // login, initialise DI, go to mainpage
                    {
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    }
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
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // Flush Sentry events when suspending
            await SentrySdk.FlushAsync(TimeSpan.FromSeconds(2));

            // TODO: Save any other application state and stop any background activity.

            deferral.Complete();
        }

        private static void OnUnobservedException(object? sender, UnobservedTaskExceptionEventArgs e) => e.SetObserved();

        [SecurityCritical]
        [HandleProcessCorruptedStateExceptions]
        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // Get a reference to the exception, because the Exception property is cleared when accessed.
            var exception = e.Exception;
            if (exception != null)
            {
                // Tell Sentry this was an unhandled exception
                exception.Data[Mechanism.HandledKey] = false;
                exception.Data[Mechanism.MechanismKey] = "Application.UnhandledException";

                // Capture the exception
                SentrySdk.CaptureException(exception);

                // Flush the event immediately
                SentrySdk.FlushAsync(TimeSpan.FromSeconds(2)).GetAwaiter().GetResult();
            }
        }

        private void CurrentDomain_FirstChanceException(object? sender, FirstChanceExceptionEventArgs e)
        {
        }
    }
}
