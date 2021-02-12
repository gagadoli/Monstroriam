using Monstroriam;
using Monstroriam.Buffs;
using Monstroriam.Items.Accessories;
using Monstroriam.Items.Placeables;
using Monstroriam.Items.Weapons.Sentry;
using Monstroriam.Items.Weapons.Summon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam
{
	public class MyPlayer : ModPlayer
	{
		public bool CrystalPower = false;
		public bool SunPower = false;
		public bool LeafPower = false;

		public bool shadowflameSummon;

		public bool CactusShield = false;
		public bool treeGuardian;
		public bool batPet;
		public bool dragonFly;
		public bool crimsonPet;

		public override void ResetEffects()
		{

			CrystalPower = false;
			SunPower = false;
			LeafPower = false;

			shadowflameSummon = false;

			CactusShield = false;
			treeGuardian = false;
			batPet = false;
			dragonFly = false;
			crimsonPet = false;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Main.myPlayer];

			if ((proj.minion || ProjectileID.Sets.MinionShot[proj.type]) && shadowflameSummon && !proj.noEnchantments)
			{
				target.AddBuff(BuffID.ShadowFlame, 90, false);
			}

			if (player.HasItem(ItemType<CursedKatana>()))
            {
				if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
                {
					if (Main.rand.NextFloat() >= .10f)
					{
						if (player.ownedProjectileCounts[mod.ProjectileType("SamuraiSlash")] < 1)
						{
							Projectile.NewProjectile(proj.Center.X - 1200, proj.Center.Y, 35f, 0f, mod.ProjectileType("SamuraiSlash"), (int)(proj.damage * 2f), knockback, player.whoAmI);
						}
					}
				}
            }
		}

		public override void PostNurseHeal(NPC nurse, int health, bool removeDebuffs, int price)
		{
			Player player = Main.player[Main.myPlayer];

			if (crimsonPet)
			{
				player.AddBuff(BuffID.Regeneration, 7200, false);
			}
		}
	}
}