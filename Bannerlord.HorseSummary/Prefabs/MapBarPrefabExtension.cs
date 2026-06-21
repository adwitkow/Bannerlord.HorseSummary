#if LOWER_THAN_1_3
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;
using System.Xml;

namespace Bannerlord.HorseSummary.Prefabs;

[PrefabExtension("MapBar", "descendant::ListPanel[@Id='BottomInfoBar']/Children")]
internal class MapBarPrefabExtension : PrefabExtensionInsertPatch
{
    public override InsertType Type => InsertType.Child;

    [PrefabExtensionXmlNode]
    public XmlNode GetNode()
    {
        var doc = new XmlDocument();
        doc.LoadXml(@"
            <HintWidget DataSource=""{MountsHint}"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" MarginLeft=""15"" Command.HoverBegin=""ExecuteBeginHint"" Command.HoverEnd=""ExecuteEndHint"">
              <Children>
                <ListPanel WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" IsEnabled=""false"">
                  <Children>
                    <Widget WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""30"" SuggestedHeight=""30"" VerticalAlignment=""Center"" Sprite=""General\TroopTypeIcons\icon_troop_type_cavalry"" />
                    <MapBarTextWidget DataSource=""{..}"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""Fixed"" MaxWidth=""65"" SuggestedHeight=""40"" VerticalAlignment=""Center"" PositionYOffset=""2"" MarginLeft=""2"" Brush=""MapTextBrushWithAnim"" Text=""@Mounts"" />
                  </Children>
                </ListPanel>
              </Children>
            </HintWidget>");
        return doc.DocumentElement!;
    }
}
#endif