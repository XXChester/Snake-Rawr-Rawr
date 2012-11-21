using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace SnakeRawrRawr.Logic {
	public class ConfigurationManager {
		#region Class variables
		// singleton variable
		private static ConfigurationManager instance = new ConfigurationManager();
		#endregion Class variables

		#region Class propeties
		public Controls PlayerOnesControls { get; set; }
		public Controls PlayerTwosControls { get; set; }
		#endregion Class properties

		#region Constructor
		public ConfigurationManager() {
			this.PlayerOnesControls = new Controls {
				Down = Keys.Down,
				Up = Keys.Up,
				Left = Keys.Left,
				Right = Keys.Right
			};
			this.PlayerTwosControls = new Controls {
				Down = Keys.S,
				Up = Keys.W,
				Left = Keys.A,
				Right = Keys.D
			};
		}
		#endregion Constructor

		#region Support methods
		public static ConfigurationManager getInstance() {
			return instance;
		}
		#endregion Support methods
	}
}
