using UnityEngine;
using System.Linq;

namespace StagedAnimation
{
    public class ModuleAnimateGenericExtra : ModuleAnimateGeneric, IScalarModule
    {
        [KSPField]
        public string deployLimitName = "";

        [KSPField]
        public bool showToggle = true;

        protected BaseEvent toggleEvt;
        protected BaseAction toggleAct;

        public override void OnAwake()
        {
            base.OnAwake();

            toggleEvt = Events["Toggle"];
            toggleAct = Actions["ToggleAction"];
        }

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);

            if (!string.IsNullOrEmpty(deployLimitName))
            {
                Fields["deployPercent"].guiName = deployLimitName;
            }

            if (!showToggle)
            {
                toggleEvt.guiActiveEditor = false;
                toggleEvt.guiActive = false;
                toggleEvt.guiActiveUnfocused = false;
                toggleEvt.guiActiveUncommand = false;
            }
        }

        public new void SetUIWrite(bool state)
        {
            toggleEvt.guiActive = state && allowManualControl && showToggle;
            toggleEvt.guiActiveUnfocused = state && allowManualControl && showToggle;
        }
    }
}
