using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace deviantart
{
	class Program
	{
		static void Main(string[] args)
		{
			{
				string workingDirectory = @"..\..\data\deviantart\mlp-vectorclub";
				Directory.CreateDirectory(workingDirectory);
				Directory.SetCurrentDirectory(workingDirectory);
			}
			WebClient webClient = new WebClient();
			foreach (var gallery in new[] {
				new { Name = "Twilight Sparkle", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29968911" },
				new { Name = "Applejack", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29968912" },
				new { Name = "Rainbow Dash", Uri="http://mlp-vectorclub.deviantart.com/gallery/29968915" },
				new { Name = "Rarity", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29968918" },
				new { Name = "Fluttershy", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29968919" },
				new { Name = "Pinkie Pie", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29968922" },
				new { Name = "With Background", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29962902" },
				new { Name = "Group", Uri = "http://mlp-vectorclub.deviantart.com/gallery/29968945" }
			})
			{
				Console.Write(gallery.Name + " 0");
				Directory.CreateDirectory(gallery.Name);
				string uri = "http://backend.deviantart.com/rss.xml?q=gallery:"
					+ Regex.Match(gallery.Uri, @"(?<=^http://).*?(?=\.)")
					+ Regex.Match(gallery.Uri, @"/\d*$");
				int images = 0;
				int errors = 0;
				int skipped = 0;
				Exception exception = null;
				while (uri != null)
				{
					try
					{
						using (var reader = XmlReader.Create(uri))
						{
							while (reader.Read())
							{
								if (reader.Name == "atom:link" && reader["rel"] == "next")
								{
									uri = reader["href"];
									break;
								}
								else if (reader.Name == "item")
								{
									uri = null;
									break;
								}
							}
							while (reader.ReadToFollowing("media:thumbnail"))
							{
								string address = reader["url"];
								while (reader.ReadToFollowing("media:content"))
									if (reader["medium"] == "document") break;
								string fileName = Path.Combine(gallery.Name,
									Regex.Match(reader["url"], @"\d*?(?=/$)") + ".png");
								try
								{
									if (!File.Exists(fileName))
										webClient.DownloadFile(address, fileName);
									else skipped++;
								}
								catch (Exception) { errors++; }
								Console.CursorLeft -= images.ToString().Length;
								Console.Write(++images);
							}
						}
					}
					catch (Exception ex) { exception = ex; }
				}
				Console.WriteLine(" images, " + skipped + " skipped, " + errors + " errors");
				if (exception != null) Console.WriteLine(exception.Message);
			}
			Console.ReadKey(true);
		}
	}
}