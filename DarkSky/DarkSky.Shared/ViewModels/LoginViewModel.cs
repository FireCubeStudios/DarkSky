using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DarkSky.Core.Classes;
using DarkSky.Services;
using FishyFlip.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.ViewModels
{
	public partial class LoginViewModel : ObservableObject
	{
		[ObservableProperty]
		private string userName;
		[ObservableProperty]
		private string password;

		private ATProtoService atProtoService;
		private INavigationService navigationService;
		private ICredentialService credentialService;
		public LoginViewModel(ATProtoService atProtoService, INavigationService navigationService, ICredentialService credentialService)
		{
			this.atProtoService = atProtoService;
			this.navigationService = navigationService;
			this.credentialService = credentialService;
		}

		[RelayCommand]
		private async Task login()
		{
			await atProtoService.LoginAsync(UserName, Password);
			if (atProtoService.Session is not null)
			{
				credentialService.SaveCredential(new Credential(atProtoService.Session.Handle.Handle, Password, atProtoService.Session.RefreshJwt));
				navigationService.NavigateTo<MainViewModel>();
			}
		}
	}
}
