using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeRawrRawr.Logic {
	public class StateManager {
		// singleton variable
		private static StateManager instance = new StateManager();

		#region Class variables
		private GameState currentGameState;
		private TransitionState currentTransitionState;
		#endregion Class variables

		#region Class propeties
		public GameState CurrentGameState {
			get { return this.currentGameState; }
			set {
				this.PreviousGameState = this.currentGameState;
				this.currentGameState = value;
			}
		}
		public GameState PreviousGameState { get; set; }
		public TransitionState CurrentTransitionState {
			get { return this.currentTransitionState; }
			set {
				this.PreviousTransitionState = this.currentTransitionState;
				this.currentTransitionState = value;
			}
		}
		public TransitionState PreviousTransitionState { get; set; }
		#endregion Class properties

		#region Constructor
		public StateManager() {
			this.currentTransitionState = TransitionState.TransitionIn;
			this.currentGameState = GameState.Waiting;

			// TESTING VALUES
			this.currentGameState = GameState.Active;
		}
		#endregion Constructor

		#region Support methods
		public static StateManager getInstance() {
			return instance;
		}
		#endregion Support methods
	}
}
