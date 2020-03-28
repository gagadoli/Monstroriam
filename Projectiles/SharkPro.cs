using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Projectiles
{

	class SharkPro : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.penetrate = -1;

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(10, 205, 255, 0) * (1f - (float)projectile.alpha / 255f);
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

			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}

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
