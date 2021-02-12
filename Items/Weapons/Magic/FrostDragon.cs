using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Magic
{
	public class FrostDragon : ModItem
	{
		public override void SetStaticDefaults() 
		{		
			Tooltip.SetDefault("Shoots [c/2295B2:Icy Fireballs]"
				+ "\n25% chance to cause [c/69D8DB:Frostburn]"
				+ "\nProvides better vision underwater"
				+ "\nCan only be used underwater");
		}

		public override void SetDefaults() 
		{
			item.damage = 15;
			item.magic = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 2;
			item.mana = 12;
			item.width = 46;
			item.height = 46;
			item.useTime = 35;
			item.useAnimation = 35;
			item.value = Item.sellPrice(0, 6, 30, 0);
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("FrostFireball");
			item.shootSpeed = 18f;
		}

		public override void HoldItem(Player player)
		{
			if (player.wet == true)
			{
				player.nightVision = true;
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.wet == true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.breath += 2;

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 48f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 35);
			recipe.AddIngredient(ItemID.FrostMinnow);
			recipe.AddTile(TileID.IceMachine);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class FrostFireball : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.magic = true;
			projectile.penetrate = 3;
			projectile.aiStyle = 1;
			projectile.timeLeft = 600;
		}

		public override void AI()
		{
			projectile.velocity.Y += 0.5f;

			if (Main.rand.NextFloat() < 4f)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width + 6, projectile.height + 6, 59, 0f, 0f, 0, default(Color), 2f);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 41, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 8;

			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(BuffID.Frostburn, 300);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}

			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}

			return false;
		}
	}
}