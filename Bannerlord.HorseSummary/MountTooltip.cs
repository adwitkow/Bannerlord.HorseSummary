using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.HorseSummary;

internal class MountTooltip
{
    private static ItemCategory[] HorseCategories = [];

    public static List<TooltipProperty> Build()
    {
        if (HorseCategories.Length == 0)
        {
            HorseCategories =
            [
                DefaultItemCategories.NobleHorse,
                DefaultItemCategories.WarHorse,
                DefaultItemCategories.Horse,
                DefaultItemCategories.PackAnimal
            ];
        }

        var party = MobileParty.MainParty;

        var horses = party.ItemRoster
            .Where(e => HorseCategories.Contains(e.EquipmentElement.Item.ItemCategory))
            .GroupBy(e => e.EquipmentElement.Item.ItemCategory)
            .Select(g => (Category: g.Key, Count: g.Sum(e => e.Amount)))
            .OrderBy(x => Array.IndexOf(HorseCategories, x.Category))
            .ToList();

        int totalHorses = horses.Sum(x => x.Count);

        var result = new List<TooltipProperty>
        {
            new TooltipProperty(new TextObject("{=Sb1MKbvP}Mounts and Pack Animals").ToString(), string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.Title),
        };

        var needingMount = party.Party.NumberOfMenWithoutHorse;
        var availableMounts = party.ItemRoster.NumberOfMounts;
        var mounted = Math.Min(availableMounts, needingMount);

        TextObject mountedTroopTextObject = new TextObject($"{mounted}/{needingMount}");
        Color color = mounted < needingMount
            ? new Color(0.82f, 0.12f, 0.07f, 1f)
            : default;

        result.Add(new TooltipProperty(
                new TextObject("{=5bSWSaPl}Footmen on horses").ToString(),
                mountedTroopTextObject.ToString(),
                0,
                color));

        result.Add(new TooltipProperty(
            string.Empty,
            string.Empty,
            0,
            false,
            TooltipProperty.TooltipPropertyFlags.RundownSeperator));

        foreach (var (category, count) in horses)
        {
            result.Add(new TooltipProperty(
                category.GetName().ToString(),
                count.ToString(),
                0));
        }

        result.Add(new TooltipProperty(
            new TextObject("{=kWVbHPtT}Total").ToString(),
            totalHorses.ToString(),
            0,
            false,
            TooltipProperty.TooltipPropertyFlags.RundownResult));

        return result;
    }
}
