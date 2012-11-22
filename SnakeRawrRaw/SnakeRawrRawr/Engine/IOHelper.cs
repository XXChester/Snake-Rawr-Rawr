using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Engine {
	public class IOHelper {
		private const string CONFIG_FILE_NAME = "config.dat";
		private const string DEFAULT_CONFIG_FILE_NAME = "defaultconfig.dat";
		/*				FILE SCHEMA
		 * LINE			VALUE
		 * 0			Music Engine Muted
		 * 1			Music Engine Volume
		 * 2			SFX Engine Muted
		 * 3			SFX Engine Volume
		 */

		public static List<string> getConfiguration() {
			List<string> configurationLines = new List<string>();
			if (!File.Exists(CONFIG_FILE_NAME)) {
				File.Copy(DEFAULT_CONFIG_FILE_NAME, CONFIG_FILE_NAME);
			}

			using (StreamReader sr = new StreamReader(CONFIG_FILE_NAME)) {
				string temp = null;
				configurationLines = new List<string>();
				while (!sr.EndOfStream) {
					temp = sr.ReadLine();
					if (temp != null && !temp.StartsWith("//")) {
						configurationLines.Add(temp);
					}
				}
			}
			return configurationLines;
		}

		public static void loadConfiguration(List<string> config) {
			SoundManager.getInstance().MusicEngine.Muted = bool.Parse(config[0]);
			SoundManager.getInstance().MusicEngine.Volume = float.Parse(config[1]);
			SoundManager.getInstance().SFXEngine.Muted = bool.Parse(config[2]);
			SoundManager.getInstance().SFXEngine.Volume = float.Parse(config[3]);
		}

		public static void resetToDefaultConfiguration() {
			File.Copy(DEFAULT_CONFIG_FILE_NAME, CONFIG_FILE_NAME, true);
			List<string> config = getConfiguration();
			loadConfiguration(config);
		}

		public static void saveCurrentConfiguration() {
			using (StreamWriter writer = new StreamWriter(CONFIG_FILE_NAME)) {
				writer.WriteLine("//Music Engine");
				writer.WriteLine(SoundManager.getInstance().MusicEngine.Muted.ToString());
				writer.WriteLine(SoundManager.getInstance().MusicEngine.Volume.ToString());
				writer.WriteLine("//SFX Engine");
				writer.WriteLine(SoundManager.getInstance().SFXEngine.Muted.ToString());
				writer.WriteLine(SoundManager.getInstance().SFXEngine.Volume.ToString());
			}
		}
	}
}
