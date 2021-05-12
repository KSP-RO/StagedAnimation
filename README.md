# StagedAnimation

This plugin adds a lightweight partmodule (ModuleStagedAnimation) that, when the part it is on is staged, plays a defined animation. It's a drop-in replacement for ModuleAnimatedDecoupler except with no decoupling. The staging icon is still a decoupler though, just so you can see one.

Based on animation-playing code by Starwaster in Animated Decouplers, full credit to them for that base.

It also adds ModuleAnimateGenericExtra which extends ModuleAnimateGeneric to support deployLimitName which if set will be the name used by the Deploy Limit slider, and showToggle which, if set to False, will hide the "toggle" event even if Deploy Limit is allowed.
