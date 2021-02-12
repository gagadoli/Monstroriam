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
	public class CursedKatana : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summoner attacks may spawn a [c/2D2D2D:Cursed Samurai]"
				+	"\nThe samurai does 2x the damage of your summon weapon or minion hitting enemies"
				+	"\nKeep this item in your inventory to take effect");
		}

		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 46;	
            item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Orange;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowOrb);
			recipe.AddIngredient(ItemID.Katana);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class SamuraiSlash : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.scale = 2f;
			projectile.timeLeft = 600;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(25, 25, 25, 120);
		}

		public override void AI()
		{
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
}