using Monstroriam;
using Monstroriam.Buffs;
using Monstroriam.Items.Accessories;
using Monstroriam.Items.Placeables;
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

		public bool VulcanoSentry = false;
		public bool MeatTower = false;
		public bool SmasherSentry = false;

		public bool CactusShield = false;
		public bool treeGuardian;

		public override void ResetEffects()
		{

			CrystalPower = false;
			SunPower = false;
			LeafPower = false;

			shadowflameSummon = false;
						
			VulcanoSentry = false;
			MeatTower = false;
			SmasherSentry = false;

			CactusShield = false;
			treeGuardian = false;
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if ((proj.minion || ProjectileID.Sets.MinionShot[proj.type]) && shadowflameSummon && !proj.noEnchantments)
			{
				target.AddBuff(BuffID.ShadowFlame, 90, false);
			}
		}
	}
}