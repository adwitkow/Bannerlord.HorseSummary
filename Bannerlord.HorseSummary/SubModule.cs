#if LOWER_THAN_1_3
using Bannerlord.UIExtenderEx;
#endif
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.HorseSummary;

public class SubModule : MBSubModuleBase
{
    protected override void OnSubModuleLoad()
    {
        var type = typeof(SubModule);
        var ns = type.Namespace;
        
        var harmony = new Harmony(ns);

#if LOWER_THAN_1_3
        var extender = UIExtender.Create(ns);
        extender.Register(type.Assembly);
        extender.Enable();
#else
        Patches.BrushFactoryPatches.Apply(harmony);
        Patches.MapInfoVMPatches.Apply(harmony);
#endif

        base.OnSubModuleLoad();
    }
}