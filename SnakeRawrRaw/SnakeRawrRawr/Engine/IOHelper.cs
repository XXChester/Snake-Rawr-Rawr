using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework.Input;

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
		 * 4			Player 1's Left
		 * 5			Player 1's Up
		 * 6		Player 1's Right
		 * 7			Player 1's Down
		 * 8			Player 2's Left
		 * 9			Player 2's Up
		 * 10			Player 2's Right
		 * 11			Player 2's Down
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
#if DEBUG
			SoundManager.getInstance().MusicEngine.Muted = true;
#endif
			SoundManager.getInstance().MusicEngine.Volume = float.Parse(config[1]);
			SoundManager.getInstance().SFXEngine.Muted = bool.Parse(config[2]);
			SoundManager.getInstance().SFXEngine.Volume = float.Parse(config[3]);
			Type type = typeof(Keys);
			ConfigurationManager.getInstance().PlayerOnesControls = new Controls {
				Left = (Keys)Enum.Parse(type, config[4], true),
				Up = (Keys)Enum.Parse(type, config[5], true),
				Right = (Keys)Enum.Parse(type, config[6], true),
				Down = (Keys)Enum.Parse(type, config[7], true),
			};

			ConfigurationManager.getInstance().PlayerTwosControls = new Controls {
				Left = (Keys)Enum.Parse(type, config[8], true),
				Up = (Keys)Enum.Parse(type, config[9], true),
				Right = (Keys)Enum.Parse(type, config[10], true),
				Down = (Keys)Enum.Parse(type, config[11], true),
			};
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
				writer.WriteLine("//Player 1 Key Bindings");
				writer.WriteLine(ConfigurationManager.getInstance().PlayerOnesControls.Left);
				writer.WriteLine(ConfigurationManager.getInstance().PlayerOnesControls.Up);
				writer.WriteLine(ConfigurationManager.getInstance().PlayerOnesControls.Right);
				writer.WriteLine(ConfigurationManager.getInstance().PlayerOnesControls.Down);
				writer.WriteLine("//Player 2 Key Bindings");
				writer.WriteLine(ConfigurationManager.getInstance().PlayerTwosControls.Left);
				writer.WriteLine(ConfigurationManager.getInstance().PlayerTwosControls.Up);
				writer.WriteLine(ConfigurationManager.getInstance().PlayerTwosControls.Right);
				writer.WriteLine(ConfigurationManager.getInstance().PlayerTwosControls.Down);
			}
		}
	}
}
