using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Buffs
{
    public class CrystalPower : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            DisplayName.SetDefault("Crystal Power");
			Description.SetDefault("More Minions!");
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.maxMinions += 1;
        }
    }
}