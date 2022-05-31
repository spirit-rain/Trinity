using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria;

namespace Trinity.Items
{
    public partial class TrinityItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.CountsAsClass(DamageClass.Melee))
                tooltips.Insert(1, new TooltipLine(Mod, "ClassHeader", "[c/fe9e23:-Warrior Class-]"));
            else if (item.CountsAsClass(DamageClass.Ranged) && !item.IsACoin)
                tooltips.Insert(1, new TooltipLine(Mod, "ClassHeader", "[c/00f2aa:-Ranger Class-]"));
            else if (item.CountsAsClass(DamageClass.Magic))
                tooltips.Insert(1, new TooltipLine(Mod, "ClassHeader", "[c/fe7ee5:-Mage Class-]"));
            else if (item.CountsAsClass(DamageClass.Summon))
                tooltips.Insert(1, new TooltipLine(Mod, "ClassHeader", "[c/00aeee:-Summoner Class-]"));

            base.ModifyTooltips(item, tooltips);
        }
    }
}
