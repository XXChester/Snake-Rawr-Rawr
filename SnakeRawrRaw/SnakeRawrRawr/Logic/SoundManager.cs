using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using GWNorthEngine.Audio;
using GWNorthEngine.Audio.Params;
using GWNorthEngine.Utils;

namespace SnakeRawrRawr.Logic {
	public class SoundManager {
		#region Class variables
		// singleton variable
		private static SoundManager instance = new SoundManager();
		private List<SoundEmitter> emitters;
		private Vector2[] lastKnownListenersPositions;
		#endregion Class variables

		#region Class propeties
		public SFXEngine SFXEngine { get; set; }
		public MusicEngine MusicEngine { get; set; }
		#endregion Class properties

		#region Constructor
		public SoundManager() {
			this.lastKnownListenersPositions = new Vector2[] { new Vector2(-10000f) };
		}
		#endregion Constructor

		#region Support methods
		public static SoundManager getInstance() {
			return instance;
		}

		public void init(ContentManager content) {
			SFXEngineParams sfxEngineParms = new SFXEngineParams();
			sfxEngineParms.Muted = true;
			this.SFXEngine = new SFXEngine(sfxEngineParms);
			this.emitters = new List<SoundEmitter>();

			MusicEngineParams musicParms = new MusicEngineParams {
				Muted = true,
				PlayList = new List<Song> {
					LoadingUtils.load<Song>(content, "SnakeRawrRawr")
				}
			};
			this.MusicEngine = new MusicEngine(musicParms);
		}

		public void addEmitter(SoundEmitter emitter) {
			this.emitters.Add(emitter);
		}

		public void removeEmitter(SoundEmitter emitter) {
			this.emitters.Remove(emitter);
		}

		public void playSoundEffect(SoundEmitter sfxEmitter, SoundEffect sfx, bool loop=false) {
			update(this.lastKnownListenersPositions);
			sfxEmitter.playSoundEffect(sfx, loop: loop);
		}

		public void update(Vector2[] listenersPositions) {
			this.MusicEngine.update();
			foreach (SoundEmitter emitter in this.emitters) {
				emitter.update(listenersPositions);
			}
			this.lastKnownListenersPositions = listenersPositions;
		}
		#endregion Support methods
	}
}
