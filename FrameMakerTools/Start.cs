using System;
using System.Collections;
using System.IO;
using System.Xml;
using Varan.FrameMaker;

namespace FrameMakerTools {
	public class Start 	{
		Varan.FrameMaker.MIF mif;
		Varan.FrameMaker.MIFSettings mifSettings;


		public Start() {
			XmlDocument configXml = new XmlDocument();
			configXml.Load("Config.xml");

			mifSettings = new Varan.FrameMaker.MIFSettings();
			mifSettings.TempFolder = configXml.SelectSingleNode("/SETTINGS/TEMP_FOLDER").InnerText;
			mifSettings.CommandFile = configXml.SelectSingleNode("/SETTINGS/COMMAND_FILE").InnerText;
			mifSettings.DZBatcherEXE = configXml.SelectSingleNode("/SETTINGS/DZBATCHER_PATH").InnerText;
			
			XmlDocument xmlDocMIFSettings = new XmlDocument();
			xmlDocMIFSettings.LoadXml(configXml.SelectSingleNode("/SETTINGS/MIF_SETTINGS").OuterXml);
			mifSettings.MIFSettingsXML = xmlDocMIFSettings;

			mif = new Varan.FrameMaker.MIF(mifSettings);
		}

		private ArrayList BuildMIFs(string sourceFolder) {
			ArrayList fmFiles = mif.GetValidFMFiles(sourceFolder);

			string tempFolder = mifSettings.TempFolder;
//			if(tempFolder == "." || tempFolder.Length == 0) {
//				tempFolder = Path.GetDirectoryName(Application.ExecutablePath);
//			}
			mif.ConvertFMtoMIF(sourceFolder, tempFolder, fmFiles);

			ArrayList mifFiles = new ArrayList();
			foreach(string file in fmFiles) {
				mifFiles.Add(Path.GetFileNameWithoutExtension(file) + ".mif");
			}
			mifFiles.Sort();

			return mifFiles;
		}

		public void IndexFromChar(string source) {
			ArrayList mifFiles = BuildMIFs(source);
			mif.AddMarkersForCharacterTags(mifSettings.TempFolder, mifFiles);
			mif.ConvertMIFtoFM(source, mifSettings.TempFolder, mifFiles);
			CleanUpMIFs(mifSettings.TempFolder, source, mifFiles);
		}

		public void IndexFromPara(string source) {
			ArrayList mifFiles = BuildMIFs(source);
			mif.AddMarkersForParagraphTags(mifSettings.TempFolder, mifFiles);
			CleanUpMIFs(mifSettings.TempFolder, source, mifFiles);
		}

		public void IndexFromKeywords(string source) {
			ArrayList mifFiles = BuildMIFs(source);
			mif.AddMarkersForKeywords(mifSettings.TempFolder, mifFiles);
			CleanUpMIFs(mifSettings.TempFolder, source, mifFiles);
		}

		public void ToPDF(string sourceBook, string targetBook) {
			mif.ConvertBookToPDF(sourceBook, targetBook);
		}

		public void All(string source, string sourceBook, string targetBook) {
			ArrayList mifFiles = BuildMIFs(source);
			mif.AddMarkersForCharacterTags(mifSettings.TempFolder, mifFiles);
			mif.AddMarkersForParagraphTags(mifSettings.TempFolder, mifFiles);
			mif.AddMarkersForKeywords(mifSettings.TempFolder, mifFiles);
			mif.ConvertBookToPDF(sourceBook, targetBook);
			CleanUpMIFs(mifSettings.TempFolder, source, mifFiles);
		}

		private void CleanUpMIFs(string tempFolder, string sourceFolder, ArrayList mifFiles) {
			//mif.ConvertMIFtoFM(mifSettings.TempFolder, sourceFolder, mifFiles);
			foreach(string mifFile in mifFiles) {
				File.Delete(tempFolder + "\\" + mifFile);
			}
		}
	}
}
