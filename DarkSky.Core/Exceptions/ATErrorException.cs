using CommunityToolkit.Mvvm.Messaging;
using DarkSky.Core.Messages;
using FishyFlip.Models;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DarkSky.Core.Exceptions
{
	/*
	 * Custom exception to handle ATError from FishyFlip
	 * Many FishyFlip operations return ATError
	 * This class formats the ATError message and contains it as an exception to be handled
	 * It also propagates the exception with WeakReferenceMessage
	 */
	public class ATErrorException : Exception
	{
		public ATError Error { get; }
		public ATErrorException(ATError Error) : base($"{Error.Detail?.Message} - {Error.StatusCode} {Error.Detail?.Error}")
		{
			this.Error = Error;

			Debug.WriteLine(Error.Detail?.Message);
			Debug.WriteLine(Error.StatusCode);
			Debug.WriteLine(Error.Detail?.Error);

			WeakReferenceMessenger.Default.Send(new ErrorMessage(this));
		}
	}
}
