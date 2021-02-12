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
	public class GhostPepper : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/776B77:Ghost Pepper] to protect your garden"
				+ "\nNeed seeds to be summoned"
				+ "\nEnemy's knockback resistance is removed on hit"
				+ "\n<right> to remove all sentries");
		}

		public override void SetDefaults()
		{
			item.damage = 5;
			item.summon = true;
			item.sentry = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 4;
			item.useAmmo = ItemID.Seed;
			item.width = 22;
			item.height = 44;
			item.useTime = 25;
			item.useAnimation = 25;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.White;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("GhostPepperSentry");
			item.scale = 0.6f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{
				X = Main.mouseX + Main.screenPosition.X,
				Y = Main.mouseY + Main.screenPosition.Y
			};

			int yOffset = 21;

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

namespace Monstroriam.Buffs
{
	public class Haunt : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("haunt");
			Description.SetDefault("AAAAAAAAAHHHH!!!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss)
			{
				npc.knockBackResist = 1f;
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class GhostPepperSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 7;
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 54;
			projectile.friendly = true;
			projectile.sentry = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 6800;
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

			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 7)
				{
					projectile.frame = 6;
				}
			}

			if (Main.GameUpdateCount % 120 == 0)
			{
				if (Utils.SentryFindTarget(projectile, out Vector2 target, out float distance))
				{
					Vector2 direction = target - projectile.Center;
					Vector2 speed = Vector2.Zero;
					speed = direction * (5f / distance);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, mod.ProjectileType("GhostPepperProj"), projectile.damage, projectile.knockBack, projectile.owner);
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 16, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndex].velocity *= 1.2f;
			}
		}
	}

	public class GhostPepperProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 56;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.minion = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 600;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
			projectile.alpha = 200;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("Haunt"), 60);
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
	}
}