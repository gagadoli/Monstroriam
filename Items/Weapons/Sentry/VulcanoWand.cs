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
	public class VulcanoWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcano Wand");
			Tooltip.SetDefault("Summons a [c/FF5605:flying Vulcano] to shoot fireballs at your enemies"
				+ "\n<right> to remove all sentries"
				+ "\nFireballs set enemies on fire and imbue your weapon with fire");
		}

		public override void SetDefaults()
		{
			item.damage = 19;
            item.summon = true;
            item.sentry = true;
            item.noMelee = true; 
            item.useStyle = 1;
			item.knockBack = 1;
			item.mana = 25;
			item.width = 54;
			item.height = 58;
			item.useTime = 25;
			item.useAnimation = 25;		
            item.value = Item.sellPrice(0, 0, 20, 40);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("VulcanoSentry");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{
				X = Main.mouseX + Main.screenPosition.X,
				Y = Main.mouseY + Main.screenPosition.Y
			};

			int yOffset = 35;

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
	public class VulcanoSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 60;
			projectile.height = 74;
			projectile.friendly = true;
			projectile.sentry = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 6800;
		}

		public override bool? CanCutTiles()
		{
			return true;
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
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}

			if (Main.GameUpdateCount % 120 == 0)
			{
				if (Utils.SentryFindTarget(projectile, out Vector2 target, out float distance))
				{
					Vector2 direction = target - projectile.Center;
					Vector2 speed = Vector2.Zero;
					speed = direction * (5f / distance);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, mod.ProjectileType("FogoProj"), projectile.damage, projectile.knockBack, projectile.owner);
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndex].velocity *= 1.2f;
			}
		}
	}

	public class FogoProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.magic = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.ignoreWater = false;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
			projectile.aiStyle = 8;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.type != NPCID.TargetDummy)
			{
				Player player = Main.player[projectile.owner];
				player.AddBuff(BuffID.WeaponImbueFire, 330);
			}
			target.AddBuff(BuffID.OnFire, 160);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);

			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndex].velocity *= 1.2f;
			}
		}
	}
}