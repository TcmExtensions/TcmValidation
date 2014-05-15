#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Shared Program
// ---------------------------------------------------------------------------------
//	Date Created	: March 15, 2014
//	Author			: Rob van Oostenrijk
// ---------------------------------------------------------------------------------
// 	Change History
//	Date Modified       : March 19, 2014
//	Changed By          : Rob van Oostenrijk
//	Change Description  : Refactored to a shared program across all Tridion native implementations
//
////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Codemesh.JuggerNET;
using Shared;
using Tridion.ContentDelivery.DynamicContent;
using Tridion.ContentDelivery.Web.Linking;

namespace TridionNative
{
	class Program
	{
		static void Main(String[] args)
		{
			try
			{
				IJvmLoader loader = JvmLoader.GetJvmLoader();
				Console.WriteLine("CodeMesh using Java: {0}", loader.JvmPath);
				Console.WriteLine();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Java virtual machine could not be loaded\n{0}", Utilities.FormatException(ex));
				Console.ReadKey();
				return;
			}

			foreach (String uri in Utilities.LinkUris)
			{
				try
				{
					using (ComponentLink componentLink = new ComponentLink(Utilities.PublicationUri))
					{
						Link link = componentLink.GetLink(uri);

						if (link != null && link.IsResolved)
							Utilities.OutputLink(uri, link.Url);
						else
							Console.WriteLine("Error resolving component link for {0}: Link is null or not resolved.", uri);
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
					using (ComponentPresentationFactory factory = new ComponentPresentationFactory(Utilities.PublicationUri))
					{
						ComponentPresentation presentation = factory.GetComponentPresentationWithHighestPriority(uri);

						if (presentation != null)
						{
							Utilities.OutputDCP(uri, presentation.Content);
						}
						else
						{
							Console.Write("Error retrieving dynamic component presentation for {0}: Presentation is null.", uri);
						}
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
