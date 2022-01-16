using UnityEngine;
using System.Linq;

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


		[KSPField(isPersistant = true)]
		public bool hasPlayed = false;

		[KSPField]
		public string fxGroupName = "decouple";

		private FXGroup fx;
		protected Animation anim;
				
		public void PlayStagedAnim()
		{
			if ((object)anim != null && !hasPlayed )
			{
				anim.Play(animationName);
				OnMoving.Fire(0f, 1f);
			}
		}


		public override void OnAwake()
		{
			this.OnMovingEvent = new EventData<float, float>("ModuleStagedAnimation.OnMovingEvent");
			this.OnStoppedEvent = new EventData<float>("ModuleAnimateDecoupler.OnStoppedEvent");

			fx = part.findFxGroup(fxGroupName);
			//if (fx == null)
			//{
			//	Debug.LogError("ModuleStagedAnimation: Cannot find fx group " + fxGroupName);
			//}
		}

		public override void OnStart(StartState state)
		{
			if (part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
			{
				part.stagingIcon = "DECOUPLER_VERT";
			}

			if (hasPlayed)
			{
				FXGroup fx = part.findFxGroup("activate");
				if (fx != null)
				{
					fx.setActive(false);
				}
			}

			//GameEvents.onVesselWasModified.Add(OnVesselWasModified);

			base.OnStart(state);
			if (!string.IsNullOrEmpty(animationName))
			{
				anim = part.FindModelAnimators(animationName).FirstOrDefault();
				if ((object)this.anim == null)
				{
					Debug.Log("ModuleStagedAnimation: Animation " + animationName + " not found");
				}
				else
				{
					this.anim[animationName].layer = layer;
					// If animation already played then set animation to end.
					if (this.hasPlayed)
					{
						this.anim[animationName].normalizedTime = 1f;
					}
				}
			}
			else
			{
				anim = null;
			}
		}

		public override void OnActive()
		{
			PlayStagedAnim();
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
				return anim[animationName].length / anim[animationName].speed;
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
			if (!string.IsNullOrEmpty(animationName))
			{
				if ((object)anim != null)
				{
					return anim.IsPlaying(animationName);
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

		public void SetScalar(float t)
		{
		}

		public void SetUIRead(bool state)
		{
		}

		public void SetUIWrite(bool state)
		{
		}
	}
}
