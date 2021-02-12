using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Summon
{
	public class BetsyStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/BC3E44:Spectral Betsy] to drop bombs");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 16;
            item.summon = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.useStyle = 5;
            item.mana = 25;
			item.width = 90;
			item.height = 90;
			item.useTime = 40;
			item.useAnimation = 40;		
            item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("BetsyProj");
			item.shootSpeed = 10f;
			item.scale = 0.6f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (speedX > 0)
			{
				speedX = 5;
			}
			else
			{
				speedX = -5;
			}

			if (speedX >= 1) 
			{
				position.X = position.X - 1200;
			}
			else
            {
				position.X = position.X + 1200;
			}
			position.Y = position.Y - 300;
			speedY = 0;

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Grenade, 15);
			recipe.AddIngredient(ItemID.AmberStaff);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	class BetsyProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.scale = 0.5f;
			projectile.timeLeft = 770;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(100, 25, 25, 120);
		}

		public override void AI()
		{
			projectile.ai[0] += 1f;

			if (projectile.ai[0] >= 30f)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-5, 5) * .5f, 1f, mod.ProjectileType("BetsyFire1"), projectile.damage, projectile.knockBack, projectile.owner);

				projectile.ai[0] = 0f;
			}

			if (++projectile.frameCounter >= 9)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}
			projectile.direction = (projectile.spriteDirection = ((projectile.velocity.X > 0f) ? 1 : -1));
			projectile.rotation = projectile.velocity.ToRotation();
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
			if (projectile.spriteDirection == -1)
				projectile.rotation += MathHelper.Pi;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int startY = frameHeight * projectile.frame;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			origin.X = (float)((projectile.spriteDirection == 1) ? (sourceRectangle.Width - 20) : 20);

			Color drawColor = projectile.GetAlpha(lightColor);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
			sourceRectangle, drawColor, projectile.rotation, origin, projectile.scale, spriteEffects, 0f);
			return false;
		}
	}

	public class BetsyFire1 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.aiStyle = 1;
			projectile.scale = 0.5f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(100, 25, 25, 200);
		}

		public override void AI()
		{
			projectile.rotation += 4;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 25f, 0f, 0f, mod.ProjectileType("BetsyFire2"), projectile.damage, projectile.knockBack, projectile.owner);
		}
	}

	public class BetsyFire2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 80;
			projectile.height = 66;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 15;
			projectile.aiStyle = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(100, 25, 25, 200);
		}

		public override void AI()
		{
			projectile.velocity.X = 0;
			projectile.velocity.Y = 0;

			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 30;
		}
	}
}