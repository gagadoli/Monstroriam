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
	public class DispenserBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/6BB600:dispensary] to drop [c/D62A2A:health]/[c/156AD3:mana]/[c/7C6454:bullets]"
				+ "\n<right> to remove all sentries");
		}

		public override void SetDefaults()
		{
            item.sentry = true;
            item.noMelee = true;
            item.useStyle = 1;
			item.mana = 35;
			item.width = 28;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 25;
            item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("Dispensary");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{
				X = Main.mouseX + Main.screenPosition.X,
				Y = Main.mouseY + Main.screenPosition.Y
			};

			int yOffset = 6;

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
	public class Dispensary : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 6;
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.friendly = true;
			projectile.sentry = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 2700;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			if (projectile.owner != Main.myPlayer)
			{
				return;
			}

			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 6)
				{
					projectile.frame = 0;
				}
			}

			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.2f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 39, projectile.velocity.X * 0.3f, -3f);
				}
			}

			if (Main.GameUpdateCount % 270 == 0)
			{
				int DisItem = -1;

				switch (Main.rand.Next(4))
				{
					case 0:
						DisItem = ItemID.Heart;
						break;

					case 1:
						DisItem = ItemID.MusketBall;
						break;

					case 2:
						DisItem = ItemID.Star;
						break;

					case 3:
						DisItem = ItemID.MusketBall;
						break;
				}
				Item.NewItem(projectile.getRect(), DisItem);
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 39, 0f, 0f, 100);
				Main.dust[dustIndex].noGravity = true;
			}
		}
	}
}