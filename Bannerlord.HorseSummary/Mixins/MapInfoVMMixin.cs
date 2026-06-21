#if LOWER_THAN_1_3
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace Bannerlord.HorseSummary.Mixins;

[ViewModelMixin(nameof(MapInfoVM.Refresh))]
public class MapInfoVMMixin : BaseViewModelMixin<MapInfoVM>
{
    private int _latestTotalMounts = -1;

    private string _mounts = "0";
    private BasicTooltipViewModel? _mountsHint;

    public MapInfoVMMixin(MapInfoVM vm) : base(vm)
    {
        MountsHint = new BasicTooltipViewModel(MountTooltip.Build);
    }

    public override void OnRefresh()
    {
        UpdateMounts(updateForced: true);
    }

    public void UpdateMounts(bool updateForced)
    {
        var roster = MobileParty.MainParty.ItemRoster;
        var totalHorses = roster.NumberOfMounts + roster.NumberOfPackAnimals;

        if (_latestTotalMounts != totalHorses || updateForced)
        {
            _latestTotalMounts = totalHorses;
            Mounts = totalHorses.ToString();
        }
    }

    [DataSourceProperty]
    public string Mounts
    {
        get => _mounts;
        set
        {
            if (_mounts != value)
            {
                _mounts = value;
                OnPropertyChangedWithValue(value, nameof(Mounts));
            }
        }
    }

    [DataSourceProperty]
    public BasicTooltipViewModel? MountsHint
    {
        get => _mountsHint;
        set
        {
            if (_mountsHint != value)
            {
                _mountsHint = value;
                OnPropertyChangedWithValue(value, nameof(MountsHint));
            }
        }
    }
}
#endif