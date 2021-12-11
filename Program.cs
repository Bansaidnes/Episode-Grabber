using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace C__episode_grabber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Episode grabber";

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            String baseurl = "https://gogoplay1.com/videos/";

            Console.WriteLine("Hello! Please enter the title you want to give your page\n");
            String title = Console.ReadLine();
        
            Console.WriteLine("\nPlease enter the show's name\n");
            String show = Console.ReadLine().Trim();
            show = show.Replace(" ", "-");
            
            Console.WriteLine("\nPlease enter the episode you want to start from\n");
            int start = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nPlease enter the episode you want to stop at (or last episode of show)\n");
            int end = Convert.ToInt32(Console.ReadLine());

            for (int i = start; i < end + 1; i++)
            {
            File.Delete("Balls.txt");
            Console.WriteLine("Current episode: ep" + i + "\n");
            String url = baseurl + show + "-episode-" + i;
                    
            WebRequest request = WebRequest.Create (url);

            request.Credentials = CredentialCache.DefaultCredentials;
       
            HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
         
            Console.WriteLine (response.StatusDescription);
         
            Stream dataStream = response.GetResponseStream ();
          
            StreamReader reader = new StreamReader (dataStream);
        
            string responseFromServer = reader.ReadToEnd ();
         
            File.WriteAllText("Balls.txt",responseFromServer);
            
            reader.Close ();
            dataStream.Close ();
            response.Close ();

            IEnumerable<string> lines = File.ReadAllLines("Balls.txt");

            string keyword = "iframe";

            IEnumerable<string> matches = !String.IsNullOrEmpty(keyword)
                                                ? lines.Where(line => line.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                                                : Enumerable.Empty<string>();

            String src = String.Format("{0}", String.Join("",matches));
            String src2 = src.Replace("<iframe src=\"//gogoplay1.com/streaming.php?id=", "").Replace("\" allowfullscreen=\"true\" frameborder=\"0\" marginwidth=\"0\" marginheight=\"0\" scrolling=\"no\"></iframe>", "").Replace("    ", "");

            Console.WriteLine("Show ID: " + src2);

             int prev = i-1;
             int next = i+1;
             String value1 = File.ReadAllText(@"value1.txt");
             String value1m =  "<title>"+title+"</title>";
             String value1f = File.ReadAllText(@"value1f.txt");
             String epis = show.Replace("-", " ");
             String value2 = "<p>You are watching: " + epis + " episode "+i+"</p>";
             String value22= "<iframe style=\"border: 3px solid #FFFFFF;\" width=\"562\" height=\"315\" sandbox=\"allow-scripts allow-same-origin allow-forms\"; "+"src=\"https://gogoplay1.com/streaming.php?id="+src2+"\" style=\"overflow:hidden;height:70%;width:50%\" height=\"70%\" width=\"50%\" scrolling=\"no\" allowfullscreen></iframe>";
             String value3 = File.ReadAllText(@"value3.txt");
             String value4 = "<button onClick=\"window.location.href='"+prev+".html';\">Previous</button>";
	         String value5 = "<button onClick=\"window.location.href='index.html';\">Episode selector</button>";
    	     String value6 = "<button onClick=\"window.location.href='"+next+".html';\">Next</button>";
             String value7 = File.ReadAllText(@"value7.txt");
             String value8 = value1 + value1m + value1f + value2 + value22 + value3 + value4 + value5 + value6 + value7;
             File.WriteAllText(i+".html", value8);

            File.Delete("Balls.txt");
            
            Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEpisode complete!\n");
             Console.ForegroundColor = ConsoleColor.DarkCyan;
            }            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nProcess complete!");
            Console.ReadKey();

        }
    }
}
