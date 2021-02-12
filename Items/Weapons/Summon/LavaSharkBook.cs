using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Summon
{
	public class LavaSharkBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a swarm of [c/DA5302:Lava Sharks]"
				+ "\nSmall chance to apply [c/494949:Oiled] or [c/FD8F4D:OnFire]");
		}

		public override void SetDefaults()
		{
			item.damage = 16;
			item.summon = true;
			item.noMelee = true;
			item.knockBack = 2;
			item.useStyle = 5;
			item.mana = 12;
			item.width = 28;
			item.height = 32;
			item.useTime = 8;
			item.useAnimation = 20;
			item.value = Item.buyPrice(0, 1, 22, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("LavaSharkPro");
			item.shootSpeed = 20f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed = speed.RotatedByRandom(MathHelper.ToRadians(10));
			speedX = speed.X;
			speedY = speed.Y;

			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 75f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<SharkBook>());
			recipe.AddIngredient(ItemID.HellstoneBar);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Bookcases);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	class LavaSharkPro : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = false;
			projectile.tileCollide = false;
			projectile.alpha = 55;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 10;
			if (Main.rand.Next(30) == 0)
			{
				target.AddBuff(BuffID.Oiled, 900);
			}

			if (Main.rand.Next(9) == 0)
			{
				target.AddBuff(BuffID.OnFire, 300);
			}
		}

		public override void AI()
		{
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 50f)
			{
				projectile.alpha += 25;
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
				}
			}
			else
			{
				projectile.alpha -= 25;
				if (projectile.alpha < 100)
				{
					projectile.alpha = 100;
				}
			}
			projectile.velocity *= 0.98f;

			if (projectile.ai[0] >= 60f)
			{
				projectile.Kill();
			}
			projectile.direction = (projectile.spriteDirection = ((projectile.velocity.X > 0f) ? 1 : -1));
			projectile.rotation = projectile.velocity.ToRotation();
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
			if (projectile.spriteDirection == -1)
				projectile.rotation += MathHelper.Pi;

			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.12f)
				{
					int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 36, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
					Main.dust[dust].noGravity = true;
				}
			}
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
			Main.spriteBatch.Draw(texture,
				projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
				sourceRectangle, drawColor, projectile.rotation, origin, projectile.scale, spriteEffects, 0f);

			return false;
		}
	}
}