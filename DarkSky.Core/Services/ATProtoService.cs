using DarkSky.Core.Classes;
using FishyFlip;
using DarkSky.Core.ViewModels;
using FishyFlip.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FishyFlip.Lexicon.Com.Atproto.Server;
using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using IdentityModel.OidcClient;
using System.Diagnostics;
using FishyFlip.Events;
using Microsoft.Extensions.DependencyInjection;
using DarkSky.Core.Services.Interfaces;
using DarkSky.Core.Exceptions;

namespace DarkSky.Core.Services
{
	public partial class ATProtoService : ObservableObject
	{
		//  .WithInstanceUrl(new Uri(Host)) for PDS https://bsky.social";
		public ATProtocol ATProtocolClient;

		private IKeyService KeyService;
		public ATProtoService(IKeyService keyService) 
		{
			KeyService = keyService;
			ATProtocolClient = CreateATProtocolClient();
		}

		/*
		 * Refreshing is weird, so do it this way, 
		 * reference: https://github.com/UnicordDev/UniSky/commit/b6ae292e176a746710df5ba731bc3761d628c608#diff-16fd45ea8b1aa9f2cce53904b52e93f0fa0e5219c96dcb818f8a61ac0dfa4008
		 */
		public async Task RefreshLoginAsync(string did, string handle, string accessJwt, string refreshJwt, string pds = "")
		{
			ATProtocolClient = CreateATProtocolClient(pds);

			// Login with refresh token
			Session session = new Session(new ATDid(did), null, new ATHandle(handle), null, refreshJwt, refreshJwt);
			var authSession = new AuthSession(session);
			Session session2 = await ATProtocolClient.AuthenticateWithPasswordSessionAsync(authSession);

			var result = await ATProtocolClient.RefreshSessionAsync();
			if(result.IsT0)
			{
				var RefreshSession = result.AsT0;
				Session? Session3 = await ATProtocolClient.AuthenticateWithPasswordSessionAsync(
					new AuthSession(
						new Session(RefreshSession.Did, RefreshSession.DidDoc, RefreshSession.Handle, null, RefreshSession.AccessJwt, RefreshSession.RefreshJwt)));
				if(Session3 is not null)
				{
					WeakReferenceMessenger.Default.Send(new AuthenticationSessionMessage(ATProtocolClient.Session));
					KeyService.Set("v1_previous_did", Session3.Did.Handler);
					KeyService.Set("v1_previous_handle", Session3.Handle.Handle);
					KeyService.Set("v1_previous_accessJWT", Session3.AccessJwt);
				}
				else
					throw new Exception($"Refresh failed Session was null");
			}
			else
				throw new Exception($"Refresh failed");
		}

		// Login with 2FA Code
		public async Task AuthCodeLoginAsync(string username, string password, string code, string pds = "")
		{
			ATProtocolClient = CreateATProtocolClient(pds);

			var result = await ATProtocolClient.CreateSessionAsync(username, password, code);
			if (result.IsT0)
			{
				var session = result.AsT0;
				await RefreshLoginAsync(session.Did.Handler, session.Handle.Handle, session.AccessJwt, session.RefreshJwt, pds);
			}
			else
				throw new ATErrorException(result.AsT1);
		}

		public async Task LoginAsync(string username, string password, string pds = "")
		{
			ATProtocolClient = CreateATProtocolClient(pds);

			var result = await ATProtocolClient.AuthenticateWithPasswordResultAsync(username, password);
			if (result.IsT0 && result.AsT0 is not null)
				WeakReferenceMessenger.Default.Send(new AuthenticationSessionMessage(ATProtocolClient.Session));
			else
				throw new ATErrorException(result.AsT1);
		}

		private void ATProtocolClient_SessionUpdated(object sender, SessionUpdatedEventArgs e)
		{
			var Session = e.Session.Session;
			KeyService.Set("v1_previous_did", Session.Did.Handler);
			KeyService.Set("v1_previous_handle", Session.Handle.Handle);
			KeyService.Set("v1_previous_accessJWT", Session.AccessJwt);
			if (e.InstanceUri is not null)
				KeyService.Set("v1_previous_pds", e.InstanceUri.ToString());
		}

		private ATProtocol CreateATProtocolClient(string pds = "")
		{
			ATProtocolBuilder Builder = new ATProtocolBuilder();
			if (!String.IsNullOrEmpty(pds))
				Builder.WithInstanceUrl(new Uri(pds));
			ATProtocol Proto = Builder.EnableAutoRenewSession(true).Build();
			Proto.SessionUpdated += ATProtocolClient_SessionUpdated;
			return Proto;
		}
	}
}
