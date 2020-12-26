using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Summon
{
	public class KrampusBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Krampus Book");
			Tooltip.SetDefault("Summons a [c/9E4A4A:krampus]");
		}

		public override void SetDefaults()
		{
			item.damage = 2;
            item.summon = true;
            item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 12;
            item.mana = 10;
			item.width = 28;
			item.height = 30;
			item.useTime = 42;
			item.useAnimation = 42;
            item.value = Item.buyPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.White;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("KrampiProj");
			item.shootSpeed = 3f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position.Y = position.Y - 45;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.Coal);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Bookcases);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	class KrampiProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 60;
			projectile.height = 128;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.aiStyle = 1;
			projectile.penetrate = 3;
			projectile.timeLeft = 620;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(100, 0, 0, 120);
		}

		public override void AI()
		{
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 8)
				{
					projectile.frame = 0;
				}
			}
			projectile.direction = (projectile.spriteDirection = ((projectile.velocity.X > 0f) ? 1 : -1));
			projectile.rotation = projectile.velocity.ToRotation();
			if (projectile.spriteDirection == -1)
				projectile.rotation += MathHelper.Pi;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity.Y = 0;
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			return false;
		}
	}
}