using CommandLine.Utility;
using System;

namespace FrameMakerTools {
	class FMTMain {

		[STAThread]
		static void Main(string[] args) {
			Arguments cmdLine = new Arguments(args);
			Start start = new Start();

			if(cmdLine["?"] != null || cmdLine["help"] != null || args.Length == 0){
				Console.WriteLine("\n\nFrameMaker Tools");
				Console.WriteLine("Usage: FMT.exe [options]");
				Console.WriteLine("\nOptions:");
				Console.WriteLine("/?, -?, help, -help   Display this help screen.");
				Console.WriteLine();
				Console.WriteLine("-source=PATH          Full path of the source folder to be processed. Due to the");
				Console.WriteLine("                      limitations of dzBatcher, this path CANNOT contain any spaces.");
				Console.WriteLine();
				Console.WriteLine("-sourceBook=xxxx      Full path and filename of a .book file. Due to the");
				Console.WriteLine("                      limitations of dzBatcher, this path CANNOT contain any spaces.");
				Console.WriteLine();
				Console.WriteLine("-targetBook=xxxx      Full path and filename of the .pdf file to create. Due to the");
				Console.WriteLine("                      limitations of dzBatcher, this path CANNOT contain any spaces.");
				Console.WriteLine();
				Console.WriteLine("-idxchar              Create index markers based on character tags.");
				Console.WriteLine("                      Requires that the \"source\" parameter is specified.");
				Console.WriteLine();
				Console.WriteLine("-idxpara              Create index markers based on paragraph tags.");
				Console.WriteLine("                      Requires that the \"source\" parameter is specified.");
				Console.WriteLine();
				Console.WriteLine("-idxkeywords          Create index markers based on keywords.");
				Console.WriteLine("                      Requires that the \"source\" parameter is specified.");
				Console.WriteLine();
				Console.WriteLine("-pdf                  Convert the entire book to a PDF document.");
				Console.WriteLine();
				Console.WriteLine("-all                  Automatically perform all of the above tasks, in order.");
				Console.WriteLine();
			} else {
				if(cmdLine["source"] != null && cmdLine["source"].ToString().IndexOf(" ") >= 0) {
					Console.WriteLine();
					Console.WriteLine("ERROR: \"source\" parameter cannot contain any spaces!");
					Environment.Exit(0);
				}
				
				if(cmdLine["book"] != null && cmdLine["book"].ToString().IndexOf(" ") >= 0) {
					Console.WriteLine();
					Console.WriteLine("ERROR: \"book\" parameter cannot contain any spaces!");
					Environment.Exit(0);
				}

				if(cmdLine["idxchar"] != null && cmdLine["all"] == null) {
					if(cmdLine["source"] == null) {
						Console.WriteLine("The \"source\" parameter is required!");
						Environment.Exit(0);
					} else {
						start.IndexFromChar(cmdLine["source"].ToString());
					}
				}

				if(cmdLine["idxpara"] != null && cmdLine["source"] != null && cmdLine["all"] == null) {
					if(cmdLine["source"] == null) {
						Console.WriteLine("The \"source\" parameter is required!");
						Environment.Exit(0);
					} else {
						start.IndexFromPara(cmdLine["source"].ToString());
					}
				}

				if(cmdLine["idxkeywords"] != null && cmdLine["source"] != null && cmdLine["all"] == null) {
					if(cmdLine["source"] == null) {
						Console.WriteLine("The \"source\" parameter is required!");
						Environment.Exit(0);
					} else {
						start.IndexFromKeywords(cmdLine["source"].ToString());
					}
				}

				if(cmdLine["pdf"] != null && cmdLine["all"] == null) {
					if(cmdLine["sourceBook"] == null || cmdLine["targetBook"] == null) {
						Console.WriteLine("The \"sourceBook\" and \"targetBook\" parameters are required!");
						Environment.Exit(0);
					} else {
						start.ToPDF(cmdLine["sourceBook"].ToString(), cmdLine["targetBook"].ToString());
					}
				}

				if(cmdLine["all"] != null && cmdLine["source"] != null && cmdLine["sourceBook"] != null && cmdLine["targetBook"] != null) {
					start.All(cmdLine["source"].ToString(), cmdLine["sourceBook"].ToString(), cmdLine["targetBook"].ToString());
				}
			}		
		}


	}
}
