#region Header
////////////////////////////////////////////////////////////////////////////////////
//
//	File Description: Shared Utilities
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
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Shared
{
	public static class Utilities
	{
		private static Regex mRegEx;

		/// <summary>
		/// Creates a <see cref="T:System.Text.RegularExpressions.RegEx" /> which is used to format newlines and carriage returns in log messages.
		/// </summary>
		/// <value>
		/// <see cref="T:System.Text.RegularExpressions.RegEx" /> formatter
		/// </value>
		private static Regex Formatter
		{
			get
			{
				if (mRegEx == null)
					mRegEx = new Regex("\r\n|\r|\n", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

				return mRegEx;
			}
		}

		public static String PublicationUri
		{
			get
			{
				try
				{
					return ConfigurationManager.AppSettings["PublicationUri"];
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to load PublicationUri from configuration\n{0}", FormatException(ex));
					return null;
				}
			}
		}

		public static IEnumerable<String> LinkUris
		{
			get
			{
				try
				{
					return ConfigurationManager.AppSettings["LinkUri"].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to load LinkUri from configuration\n{0}", FormatException(ex));
					return null;
				}
			}
		}

		public static IEnumerable<String> BrokerUris
		{
			get
			{
				try
				{
					return ConfigurationManager.AppSettings["BrokerUri"].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to load BrokerUri from configuration\n{0}", FormatException(ex));
					return null;
				}
			}
		}

		public static String FormatXml(String xml)
		{
			if (String.IsNullOrEmpty(xml))
				return "Content:\n\t*No Content*";

			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);

				XmlWriterSettings settings = new XmlWriterSettings()
				{
					OmitXmlDeclaration = true,
					Indent = true,
					IndentChars = "\t",
					NewLineChars = "\n\t",
					NewLineHandling = System.Xml.NewLineHandling.Replace,
					NewLineOnAttributes = true
				};

				using (StringWriter sw = new StringWriter())
				{
					using (XmlWriter xw = XmlTextWriter.Create(sw, settings))
					{
						xmlDocument.WriteTo(xw);
					}

					return String.Format("Content:\n\t{0}", sw.ToString());
				}
			}
			catch
			{
				// Maybe it was not XML...
				return String.Format("Content:\n\t{0}", xml); 
			}
		}

		/// <summary>
		/// Generate an indented log trace for the given <see cref="T:System.Exception" />
		/// </summary>
		/// <param name="ex"><see cref="T:System.Exception" /></param>
		/// <returns><see cref="T:System.Exception" /> indented log trace</returns>
		public static String FormatException(Exception exception)
		{
			StringBuilder sbMessage = new StringBuilder();
			int depth = 1;

			if (exception != null)
			{
				if (!String.IsNullOrEmpty(exception.Source))
					sbMessage.AppendFormat("{0} ({1})\n", exception.GetType().FullName, exception.Source);

				if (!String.IsNullOrEmpty(exception.Message))
					sbMessage.AppendLine(exception.Message);

				if (!String.IsNullOrEmpty(exception.StackTrace))
					sbMessage.AppendLine(exception.StackTrace);

				while (exception.InnerException != null)
				{
					String indent = new String('\t', depth);

					exception = exception.InnerException;

					if (!String.IsNullOrEmpty(exception.Source))
					{
						sbMessage.Append(indent);
						sbMessage.AppendFormat("{0} ({1})\n", exception.GetType().FullName, exception.Source);
					}

					if (!String.IsNullOrEmpty(exception.Message))
					{
						sbMessage.Append(indent);
						sbMessage.AppendLine(exception.Message);
					}

					if (!String.IsNullOrEmpty(exception.StackTrace))
					{
						sbMessage.Append(indent);
						sbMessage.AppendLine(Formatter.Replace(exception.StackTrace, "\r" + indent));
					}

					depth++;
				}
			}

			return sbMessage.ToString();
		}

		public static void OutputLink(String uri, String url)
		{
			Console.WriteLine("Resolving link {0}", uri);
			Console.WriteLine();

			if (String.IsNullOrEmpty(url))
				Console.WriteLine("\tFailed to resolve link");
			else
				Console.WriteLine("\t{0}", url);

			Console.WriteLine();
		}

		public static void OutputDCP(String uri, String content)
		{
			Console.WriteLine("Retrieving DCP {0}", uri);
			Console.WriteLine();
			Console.WriteLine(FormatXml(content));
			Console.WriteLine();
		}
	}
}
