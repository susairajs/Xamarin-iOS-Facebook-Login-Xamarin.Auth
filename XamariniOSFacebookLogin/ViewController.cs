using System;

using UIKit;
using Xamarin.Auth;
using System.Json;
using Foundation;

namespace XamariniOSFacebookLogin
{
	public partial class ViewController : UIViewController
	{
	        // Base Urls 
	        string authUrl = "https://m.facebook.com/dialog/oauth/";
		string loginSuccessUrl = "http://www.facebook.com/connect/login_success.html";
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}


		// Button event 
		partial void UIButton8_TouchUpInside(UIButton sender)
		{
			var authendication = new OAuth2Authenticator(
				clientId: "1843461312586790",
				scope: "",
				authorizeUrl: new Uri(authUrl),
				redirectUrl: new Uri(loginSuccessUrl));
			authendication.Completed+= Auth_Completed;
			var ui = authendication.GetUI();
			PresentViewController(ui, true, null);
		}
		
		//Respose method for auth reponse
		private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				var request = new OAuth2Request("GET", 
				                                new Uri("https://graph.facebook.com/me?fields=name,picture,cover,birthday")
				                                , null, e.Account);
				var response = await request.GetResponseAsync();
				var user = JsonValue.Parse(response.GetResponseText());
				var fbName = user["name"];
				var fbCover = user["cover"]["source"];
				var fbProfile = user["picture"]["data"]["url"];
				lblName.Text = fbName.ToString();
				imgCover.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(fbCover)));
				imgProfile.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(fbProfile)));
			}
                         DismissViewController(true, null);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
