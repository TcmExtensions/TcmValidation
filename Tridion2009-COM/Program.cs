#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Tridion COM Program
// ---------------------------------------------------------------------------------
//	Date Created	: March 15, 2014
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
// 	Change History
//	Date Modified       : 
//	Changed By          : 
//	Change Description  : 
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Shared;
using Tridion.ContentDelivery.Broker;
using Tridion.ContentDelivery.Linking;

namespace TridionCOM
{
	class Program
	{
		static void Main(string[] args)
		{
			Regex regex = new Regex(@"href=\""(?<url>.*?)\""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

			foreach (String uri in Utilities.LinkUris)
			{
				try
				{
					using (ComponentLink componentLink = new ComponentLink())
					{
						String url = componentLink.GetLinkAsString("tcm:0-0-0", uri, "tcm:0-0-0", String.Empty, String.Empty, false, false);

						if (!String.IsNullOrEmpty(url))
						{
							Match match = regex.Match(url);

							if (match.Success)
								Utilities.OutputLink(uri, match.Groups["url"].Value);
							else
								Console.WriteLine("Error resolving component link for {0}: Invalid url {1}.", uri, url);
						}
						else
						{
							Console.WriteLine("Error resolving component link for {0}: Link is null or not resolved.", uri);
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error resolving component link for {0}:\n{1}", uri, Utilities.FormatException(ex));
				}
			}

			foreach (String uri in Utilities.BrokerUris)
			{
				try
				{
					using (ComponentPresentationFactory factory = new ComponentPresentationFactory())
					{
						ComponentPresentation presentation = factory.GetComponentPresentationWithHighestPriority(uri);

						if (presentation != null)
							Utilities.OutputDCP(uri, presentation.Content);
						else
							Console.WriteLine("Error retrieving dynamic component presentation for {0}: Presentation is null.", uri);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error retrieving dynamic component presentation for {0}:\n{1}", uri, Utilities.FormatException(ex));
				}
			}

			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}
	}
}
