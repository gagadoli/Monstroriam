using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monstroriam.Items.Weapons.Magic
{
	public class DefenderRod : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Shoots a [c/A806FF:Fist] from the sky" +
				"\nAfter landing spawns another [c/A806FF:Fist] that deals double damage" +
				"\nMust be used in open spaces");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 11;
			item.magic = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 5;
			item.mana = 14;
			item.width = 48;
			item.height = 48;
			item.useTime = 55;
			item.useAnimation = 55;
			item.value = Item.sellPrice(0, 0, 95, 0);
			item.rare = ItemRarityID.White;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("MonkFist");
			item.shootSpeed = 20f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1;
			for (int index = 0; index < numberProjectiles; ++index)
			{
				Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(100) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));
				vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-100, 100);
				vector2_1.Y -= (float)(100 * index);
				float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
				float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
				if ((double)num13 < 0.0) num13 *= -1f;
				if ((double)num13 < 20.0) num13 = 20f;
				float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
				float num15 = item.shootSpeed / num14;
				float num16 = num12 * num15;
				float num17 = num13 * num15;
				float SpeedX = num16 + (float)Main.rand.Next(-12, 10) * 0.100f;
				float SpeedY = num17 + (float)Main.rand.Next(-12, 10) * 0.100f;
				Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, (float)Main.rand.Next(5));
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LargeAmethyst);
			recipe.AddIngredient(ItemID.AleThrowingGlove);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class MonkFist : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 100;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(SoundID.NPCDeath3, projectile.position);
			projectile.Kill();
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 25; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width + 20, projectile.height + 20, 62, 0f, 0f, 100, default(Color), 1.7f);
				Main.dust[dustIndex].noGravity = true;
			}

			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 1f, 1f, -20f, mod.ProjectileType("MonkFist2"), projectile.damage * 2, projectile.knockBack + 5, projectile.owner);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}

	public class MonkFist2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 38;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 30;
			projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 25; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width + 20, projectile.height + 20, 62, 0f, 0f, 100, default(Color), 1.7f);
				Main.dust[dustIndex].noGravity = true;
			}
			Main.PlaySound(SoundID.NPCDeath9, projectile.position);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}