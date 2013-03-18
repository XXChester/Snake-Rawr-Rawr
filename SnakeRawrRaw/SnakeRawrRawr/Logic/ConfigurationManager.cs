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
		private Controls playerOnesControls;
		private Controls playerTwosControls;
		#endregion Class variables

		#region Class propeties
		public Controls PlayerOnesControls { get{return this.playerOnesControls;} set{this.playerOnesControls = value;} }
		public Controls PlayerTwosControls { get { return this.playerTwosControls; } set { this.playerTwosControls = value; } }
		#endregion Class properties

		#region Constructor
		public ConfigurationManager() {
		}
		#endregion Constructor

		#region Support methods
		public static ConfigurationManager getInstance() {
			return instance;
		}
		#endregion Support methods
	}
}
