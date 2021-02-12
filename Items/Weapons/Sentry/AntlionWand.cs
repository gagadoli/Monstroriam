using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Sentry
{
	public class AntlionWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/D8A768:Antlion Nest] to spawn larvas when destroyed" 
				+ "\n<right> to destroy all sentries");
		}

		public override void SetDefaults()
		{
			item.damage = 8;
            item.summon = true;
            item.sentry = true;
            item.noMelee = true;
            item.useStyle = 1;
			item.knockBack = 6;
			item.mana = 20;
			item.width = 40;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;
            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("AntlionSentry");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sandstone, 35);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{ X = Main.mouseX + Main.screenPosition.X, Y = Main.mouseY + Main.screenPosition.Y };

			int yOffset = -4;

			Vector2 spawnPosition = Utils.FindSentrySpawnSpot(player, worldStartPosition, yOffset);

			Projectile.NewProjectile(spawnPosition.X, spawnPosition.Y, 0f, 0f, item.shoot, item.damage, item.knockBack, player.whoAmI);

			player.UpdateMaxTurrets();

			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class AntlionSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 26;
			projectile.friendly = true;
			projectile.sentry = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 1200;
		}

		public override bool? CanCutTiles()
		{
			return true;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return true;
		}

		public override void AI()
		{
			if (projectile.owner != Main.myPlayer)
			{
				return;
			}

			if (++projectile.frameCounter >= 9)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 7, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndex].velocity *= 1.2f;
			}

			for (int i = 0; i < 3; i++)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 16f, Main.rand.Next(-10, 11) * .25f, Main.rand.Next(-10, -5) * .25f, mod.ProjectileType("AntlionProj"), projectile.damage, projectile.knockBack, projectile.owner);
			}
		}
	}

	public class AntlionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 720;
			projectile.aiStyle = 1;
		}

		public override void AI()
		{
			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 6)
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