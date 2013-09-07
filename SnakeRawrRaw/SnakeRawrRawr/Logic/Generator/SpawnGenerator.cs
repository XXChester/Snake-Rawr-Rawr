using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace SnakeRawrRawr.Logic.Generator {
	public class SpawnGenerator {
		#region Class variables
		// singleton variable
		private static SpawnGenerator instance = new SpawnGenerator();

		private Thread spawnerThread;
		public delegate void HandleSpawn();
		#endregion Class variables

		#region Class propeties
		public Queue<HandleSpawn> SpawnRequests { get; set; }
		public bool Running { get; set; }
		#endregion Class properties

		#region Constructor
		public SpawnGenerator() {
			ThreadStart threadStart = new ThreadStart(processSpawnRequests);
			this.SpawnRequests = new Queue<HandleSpawn>();
			this.Running = true;
			this.spawnerThread = new Thread(threadStart);
			this.spawnerThread.Start();
		}
		#endregion Constructor

		#region Support methods
		private void processSpawnRequests() {
			HandleSpawn spawnHandler = null;
			do {
				if (SpawnRequests.Count > 0) {
					spawnHandler = SpawnRequests.Dequeue();
					spawnHandler.Invoke();
				} else {
					Thread.Sleep(100);
				}
			} while (Running);
		}

		public static SpawnGenerator getInstance() {
			return instance;
		}
		#endregion Support methods

		~SpawnGenerator() {
			this.Running = false;
			this.spawnerThread.Abort();
			this.spawnerThread = null;
		}
	}
}
