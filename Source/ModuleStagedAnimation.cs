using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace StagedAnimation
{
	public class ModuleStagedAnimation : PartModule, IScalarModule
	{
		[KSPField]
		public string animationName = "";

		[KSPField]
		public int layer = 0;

		[KSPField]
		public string moduleID = "stagedAnimation";


		[KSPField( isPersistant = true )]
		public bool hasPlayed = false;

		[KSPField]
		public string fxGroupName = "decouple";

		private FXGroup fx;
		protected Animation[] anims;

		public void PlayStagedAnim()
		{
			if( this.anims != null && !this.hasPlayed )
			{
				for( int i = 0; i < this.anims.Length; i++ )
				{
					Debug.Log( "ANIMKAT playing anim on " + this.anims[i].gameObject.name + "" );
					this.anims[i].Play( this.animationName );
				}
				this.OnMoving.Fire( 0f, 1f );
			}
		}


		public override void OnAwake()
		{
			this.OnMovingEvent = new EventData<float, float>( "ModuleStagedAnimation.OnMovingEvent" );
			this.OnStoppedEvent = new EventData<float>( "ModuleAnimateDecoupler.OnStoppedEvent" );

			this.fx = this.part.findFxGroup( this.fxGroupName );
			if( this.fx == null )
			{
				Debug.LogError( "ModuleStagedAnimation: Cannot find fx group " + this.fxGroupName );
			}
		}

		public override void OnStart( StartState state )
		{
			if( this.part.stagingIcon == string.Empty && this.overrideStagingIconIfBlank )
			{
				this.part.stagingIcon = "DECOUPLER_VERT";
			}

			if( this.hasPlayed )
			{
				FXGroup fx = part.findFxGroup( "activate" );
				if( fx != null )
				{
					fx.setActive( false );
				}
			}

			//GameEvents.onVesselWasModified.Add(OnVesselWasModified);

			base.OnStart( state );
			if( !string.IsNullOrEmpty( animationName ) )
			{
				List<Transform> modelNodes = this.part.FindModelNodes();

				// find all animation modules
				List<Animation> foundAnims = new List<Animation>();
				for( int i = 0; i < modelNodes.Count; i++ )
                {
					Part.FindModelComponents<Animation>( modelNodes[i], string.Empty, foundAnims ); // this does a recursive search.
				}

				// get the animations that match the specified name.
				List<Animation> matchedAnims = new List<Animation>();
				for( int i = 0; i < foundAnims.Count; i++ )
                {
					if( foundAnims[i].GetClip( this.animationName ) == null )
                    {
						continue;
                    }

					matchedAnims.Add( foundAnims[i] );
                }

				this.anims = foundAnims.ToArray();
				if( this.anims == null )
				{
					Debug.LogWarning( "ModuleStagedAnimation: Animation " + this.animationName + " not found" );
				}
				else
				{
					Debug.Log( $"ANIMKAT found {this.anims.Length} anims on {this.gameObject.name}" );
					for( int i = 0; i < this.anims.Length; i++ )
					{

						this.anims[i][animationName].layer = layer;
						// If animation already played then set animation to end.
						if( this.hasPlayed )
						{
							this.anims[i][animationName].normalizedTime = 1f;
						}
					}
				}
			}
			else
			{
				this.anims = null;
			}
		}

		public override void OnActive()
		{
			this.PlayStagedAnim();
		}

		// TODO ideally, check if we're on a new vessel and fire the anim?
		// But for now, we don't care.
		//private void OnVesselWasModified(Vessel v)
		//{
		//	if ((object)v != null && v == vessel)
		//	{
		//		if (!(isDecoupling || hasPlayed))
		//		{
		//			if ((object)part.FindAttachNode(this.explosiveNodeID).attachedPart == null)
		//			{
		//				isDecoupling = true;
		//				OnMoving.Fire(0f, 1f);
		//				OnStop.Fire(1f);
		//			}
		//		}
		//	}
		//}

		//private void OnDestroy()
		//{
		//	GameEvents.onVesselWasModified.Remove(OnVesselWasModified);
		//}

		//
		// Properties
		//
		private EventData<float, float> OnMovingEvent;
		private EventData<float> OnStoppedEvent;

		float EventTime
		{
			get
			{
				return anims[0][animationName].length / anims[0][animationName].speed;
			}
		}

		public bool CanMove
		{
			get
			{
				return true;
			}
		}

		public float GetScalar
		{
			get
			{
				return hasPlayed ? 1f : 0f;
			}
		}

		public string ScalarModuleID
		{
			get
			{
				return this.moduleID;
			}
		}

		public EventData<float, float> OnMoving
		{
			get
			{
				return OnMovingEvent;
			}
		}

		public EventData<float> OnStop
		{
			get
			{
				return OnStoppedEvent;
			}
		}

		//
		// Methods
		//
		public bool IsMoving()
		{
			if( !string.IsNullOrEmpty( animationName ) )
			{
				if( anims != null )
				{
					return anims[0].IsPlaying( animationName );
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public void SetScalar( float t )
		{
		}

		public void SetUIRead( bool state )
		{
		}

		public void SetUIWrite( bool state )
		{
		}
	}
}