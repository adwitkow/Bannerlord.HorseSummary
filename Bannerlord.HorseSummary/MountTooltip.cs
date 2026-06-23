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
    private static readonly Color RedColor = new Color(0.82f, 0.12f, 0.07f, 1f);

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
            CreateTitleLine(),
            CreateMountedFootmenLine(party),
            CreateHerdingLine(party),
            CreateSeparatorLine()
        };

        foreach (var (category, count) in horses)
        {
            result.Add(CreateCategoryLine(category, count));
        }

        result.Add(CreateTotalLine(totalHorses));

        return result;
    }

    private static TooltipProperty CreateTotalLine(int totalHorses)
    {
        return new TooltipProperty(
            new TextObject("{=kWVbHPtT}Total").ToString(),
            totalHorses.ToString(),
            0,
            false,
            TooltipProperty.TooltipPropertyFlags.RundownResult);
    }

    private static TooltipProperty CreateCategoryLine(ItemCategory category, int count)
    {
        return new TooltipProperty(
            category.GetName().ToString(),
            count.ToString(),
            0);
    }

    private static TooltipProperty CreateTitleLine()
    {
        return new TooltipProperty(new TextObject("{=Sb1MKbvP}Mounts and Pack Animals").ToString(), string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.Title);
    }

    private static TooltipProperty CreateSeparatorLine()
    {
        return new TooltipProperty(
            string.Empty,
            string.Empty,
            0,
            false,
            TooltipProperty.TooltipPropertyFlags.RundownSeperator);
    }

    private static TooltipProperty CreateMountedFootmenLine(MobileParty party)
    {
        var needingMount = party.Party.NumberOfMenWithoutHorse;
        var availableMounts = party.ItemRoster.NumberOfMounts;
        var mounted = Math.Min(availableMounts, needingMount);

        TextObject mountedTroopTextObject = new TextObject($"{mounted}/{needingMount}");
        Color color = needingMount > mounted
            ? RedColor
            : default;

        return new TooltipProperty(
            new TextObject("{=5bSWSaPl}Footmen on horses").ToString(),
            mountedTroopTextObject.ToString(),
            0,
            color);
    }

    private static TooltipProperty CreateHerdingLine(MobileParty party)
    {
        var totalManCount = party.MemberRoster.TotalManCount;
        var herdSize = party.ItemRoster.NumberOfPackAnimals
            + party.ItemRoster.NumberOfLivestockAnimals;

        TextObject herdingTextObject = new TextObject($"{herdSize}/{totalManCount}");
        Color color = herdSize > totalManCount
            ? RedColor
            : default;

        return new TooltipProperty(
            new TextObject("{=NhAMSaWU}Herding").ToString(),
            herdingTextObject.ToString(),
            0,
            color);
    }
}
