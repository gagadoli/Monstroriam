using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Buffs
{
    public class ProtectiveOrb : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Protective Orb");
			Description.SetDefault("5% Reduced incoming damage");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.05f;
        }
    }
}