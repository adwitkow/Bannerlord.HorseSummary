#if !LOWER_THAN_1_3
using HarmonyLib;
using HarmonyLib.PatchBuilder;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar;

namespace Bannerlord.HorseSummary.Patches;

internal class MapInfoVMPatches
{
    private static MapInfoItemVM MountsVm = default!;

    public static void Apply(Harmony harmony)
    {
        harmony.Patch<MapInfoVM>()
            .Method("CreateItems")
                .Postfix(CreateItemsPostfix)
            .Method("UpdatePlayerInfo")
                .Postfix(UpdatePlayerInfoPostfix);
    }

    private static void CreateItemsPostfix(MapInfoVM __instance)
    {
        MountsVm = new MapInfoItemVM("mounts", MountTooltip.Build);
        __instance.SecondaryInfoItems.Add(MountsVm);
    }
    
    private static void UpdatePlayerInfoPostfix(MapInfoVM __instance, bool updateForced)
    {
        var roster = MobileParty.MainParty.ItemRoster;
        var totalHorses = roster.NumberOfMounts + roster.NumberOfPackAnimals;

        if (MountsVm.IntValue != totalHorses || updateForced)
        {
            MountsVm.IntValue = totalHorses;
            MountsVm.Value = totalHorses.ToString();
        }
    }
}
#endif