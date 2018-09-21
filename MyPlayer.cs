using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace Monstroriam
{
	public class MyPlayer : ModPlayer
	{
		public bool CrystalPower = false;
		public bool SunPower = false;
		public bool CactusShield = false;
		public bool VulcanoSentry = false;
		public bool MeatTower = false;
		public bool shadowflameSummon;

		public override void ResetEffects()
		{
			CrystalPower = false;
			SunPower = false;
			CactusShield = false;	
			VulcanoSentry = false;
			shadowflameSummon = false;
			MeatTower = false;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{			
			if ((proj.minion || ProjectileID.Sets.MinionShot[proj.type]) && shadowflameSummon && !proj.noEnchantments)
			{
				target.AddBuff(BuffID.ShadowFlame, 90, false);
			}
		}
	}

	namespace Items.Accessories
	{
		[AutoloadEquip(EquipType.Neck)]
		class ShadowflameScarf : ModItem
		{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Shadowflame Scarf");
				Tooltip.SetDefault("Minion's projectiles will cause Shadowflame debuff");
			}
			public override void SetDefaults()
			{
				item.width = 18;
				item.height = 30;
				item.accessory = true;
				item.value = 15000;
				item.rare = 5;
			}

			public override void UpdateAccessory(Player player, bool hideVisual)
			{
				player.GetModPlayer<MyPlayer>().shadowflameSummon = true;
			}
		}
	}
}
