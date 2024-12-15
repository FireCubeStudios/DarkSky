using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DarkSky.Controls
{
	/*
	 * Control to display an image which when clicked will request to show an image overlay
	 * The control will contain a list of images to send to the ImageOverlay
	 * The control will also contain the singular image it is displaying
	 */
	public sealed partial class InteractiveImage : UserControl
	{
		public InteractiveImage()
		{
			this.InitializeComponent();
		}
	}
}
