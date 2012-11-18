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

namespace SnakeRawrRawr.Engine {
	public class SoundManager {
		#region Class variables
		// singleton variable
		private static SoundManager instance = new SoundManager();
		private List<SoundEmitter> emitters;
		#endregion Class variables

		#region Class propeties
		public SFXEngine SFXEngine { get; set; }
		public SFXEngine MusicEngine { get; set; }
		#endregion Class properties

		#region Constructor
		public SoundManager() {
			
		}
		#endregion Constructor

		#region Support methods
		public static SoundManager getInstance() {
			return instance;
		}

		public void init(ContentManager content) {
			SFXEngineParams sfxEngineParms = new SFXEngineParams();
			sfxEngineParms.Muted = false;
			this.SFXEngine = new SFXEngine(sfxEngineParms);
			this.emitters = new List<SoundEmitter>();

			// used a SoundEffectEngine because we are looping the same track
			//MusicEngineParams musicParms = new MusicEngineParams {
			SFXEngineParams musicParms = new SFXEngineParams {
				Muted = false,
				/*PlayList = new List<Song> {
					LoadingUtils.load<Song>(content, "SnakeRawrRawr")
				}*/
			};
			this.MusicEngine = new SFXEngine(musicParms);
			//this.MusicEngine = new MusicEngine(musicParms);
			this.MusicEngine.playSoundEffect(LoadingUtils.load<SoundEffect>(content, "SnakeRawrRawr"), loop: true);
		}

		public void addEmitter(SoundEmitter emitter) {
			this.emitters.Add(emitter);
		}

		public void update(Vector2 playersPosition) {
			this.MusicEngine.update();
			foreach (SoundEmitter emitter in this.emitters) {
				emitter.update(playersPosition);
			}
		}
		#endregion Support methods
	}
}
