using Monstroriam;
using Monstroriam.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Summon
{
	public class GnomeBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons 4 running [c/FF0000:G][c/FFEE00:n][c/26FF00:o][c/00FFCB:m][c/0077FF:e][c/9000FF:s]");
		}

		public override void SetDefaults() 
		{
			item.damage = 5;
			item.summon = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 2;
			item.mana = 15;
			item.width = 28;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 80;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("GnomeRun");
			item.shootSpeed = 5f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedY = speedY/2;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RedBrick, 15);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class GnomeRun : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 6;
			projectile.timeLeft = 720;
			projectile.aiStyle = 1;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.12f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				}
			}

			if (++projectile.frameCounter >= 4)
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
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}

			projectile.velocity.Y = 0;
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			return false;
		}
	}
}