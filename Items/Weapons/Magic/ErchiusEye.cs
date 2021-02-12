using Monstroriam.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Magic
{
	public class ErchiusEye : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Spray [c/C87BE3:Erchius Dust] that slows enemies");
		}

		public override void SetDefaults() {
			item.damage = 4;
			item.noMelee = true;
			item.magic = true;
			item.autoReuse = true;
			item.mana = 5;
			item.crit = 15;
			item.rare = ItemRarityID.Pink;
			item.width = 28;
			item.height = 30;
			item.useTime = 2;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.shootSpeed = 8f;
			item.shoot = ProjectileType<ErchiusLaser>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Lens, 2);
			recipe.AddIngredient(ItemID.PinkGel, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class ErchiusLaser : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 55;
			projectile.extraUpdates = 3;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
			if (projectile.timeLeft > 55)
			{
				projectile.timeLeft = 55;
			}
			if (projectile.ai[0] > 1f)
			{
				if (Main.rand.Next(3) == 0)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 255, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 130, default(Color), 1.8f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 2.5f;
				}
			}
			else
			{
				projectile.ai[0] += 1f;
			}
			return;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("SlowDebuff"), 150);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}
	}
}