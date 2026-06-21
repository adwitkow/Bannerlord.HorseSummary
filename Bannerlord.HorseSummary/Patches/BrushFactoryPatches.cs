#if !LOWER_THAN_1_3
using HarmonyLib;
using HarmonyLib.PatchBuilder;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;

namespace Bannerlord.HorseSummary.Patches;

internal class BrushFactoryPatches
{
    public static void Apply(Harmony harmony)
    {
        harmony.Patch<BrushFactory>()
            .Method(x => x.GetBrush(default))
                .Postfix(GetBrushPostfix);
    }

    private static void GetBrushPostfix(BrushFactory __instance, Brush __result)
    {
        if (__result.Name != "MapBar.Right.Icons" || __result.GetLayer("mounts") is not null)
        {
            return;
        }

        __result.AddLayer(new BrushLayer()
        {
            Name = "mounts",
            Sprite = UIResourceManager.SpriteData.GetSprite(@"General\TroopTypeIcons\icon_troop_type_cavalry"),
            ImageFitType = ImageFit.ImageFitTypes.Contain,
            ImageFitVerticalAlignment = ImageFit.ImageVerticalAlignments.Center,
            ImageFitHorizontalAlignment = ImageFit.ImageHorizontalAlignments.Center,
            ExtendBottom = -7,
            ExtendTop = -7,
            ExtendLeft = -7,
            ExtendRight = -7
        });
    }
}
#endif