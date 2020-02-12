using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Buffs
{
    public class SunPower : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            DisplayName.SetDefault("Crystal Power");
			Description.SetDefault("Resist to cold");
        }

        public override void Update(Player player, ref int buffIndex)
        {
			player.resistCold = true;
        }
    }
}