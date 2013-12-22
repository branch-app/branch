using System;
using System.Web;

namespace Branch.App.Helpers.Razor
{
	public class SocialHelpers
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="page"></param>
		/// <param name="via"></param>
		/// <param name="hashtag"></param>
		/// <returns></returns>
		public static string GenerateTwitterShareLink(string text, string page, string via = "alexerax",
			string hashtag = "branchapp")
		{
			var url = "https://twitter.com/intent/tweet?original_referer={0}&text={1}&tw_p=tweetbutton&url={0}";
			url = string.Format(url, HttpUtility.HtmlEncode(page), text);
			if (String.IsNullOrEmpty(via)) url += string.Format("&via={0}", via);
			if (String.IsNullOrEmpty(hashtag)) url += string.Format("&hashtags={0}", hashtag);

			return url;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static string GenerateFacebookShareLink(string page)
		{
			return string.Format("https://www.facebook.com/sharer/sharer.php?u={0}", HttpUtility.HtmlEncode(page));
		}
	}
}