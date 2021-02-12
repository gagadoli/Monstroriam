using Monstroriam;
using Monstroriam.Projectiles;
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
	public class CultistIdol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcano Wand");
			Tooltip.SetDefault("Summons a [c/909090:Cultist Statue] that spawn mini pillars based on your weapon"
				+ "\n[c/B38B67:Solar Pillar] Increases your defense"
				+ "\n[c/5E847E:Vortex Pillar] Increases crit chance"
				+ "\n[c/9C5984:Nebula Pillar] Decreases your mana cost"
				+ "\n[c/598399:Stardust Pillar] Increases your max sentry capacity"
				+ "\n<right> to remove all sentries");
		}

		public override void SetDefaults()
		{
            item.sentry = true;
            item.noMelee = true; 
            item.useStyle = 1;
			item.mana = 25;
			item.width = 38;
			item.height = 48;
			item.useTime = 25;
			item.useAnimation = 25;
            item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("CultistStatue");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{
				X = Main.mouseX + Main.screenPosition.X,
				Y = Main.mouseY + Main.screenPosition.Y
			};

			int yOffset = 43;

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
	public class CultistStatue : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 76;
			projectile.height = 122;
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
			Player player = Main.player[projectile.owner];

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

			if (Main.GameUpdateCount % 1800 == 0)
			{
				int projId = -1;

				if (player.inventory[player.selectedItem].melee)
				{
					projId = ProjectileType<CultistPillar1>();
					player.AddBuff(mod.BuffType("CultistBuff1"), 1800);
				}
				else if (player.inventory[player.selectedItem].ranged)
				{
					projId = ProjectileType<CultistPillar2>();
					player.AddBuff(mod.BuffType("CultistBuff2"), 1800);
				}
				else if (player.inventory[player.selectedItem].magic)
				{
					projId = ProjectileType<CultistPillar3>();
					player.AddBuff(mod.BuffType("CultistBuff3"), 1800);
				}
				else if (player.inventory[player.selectedItem].summon)
				{
					projId = ProjectileType<CultistPillar4>();
					player.AddBuff(mod.BuffType("CultistBuff4"), 1800);
				}
				else
                {
					projId = ProjectileType<CultistPillar4>();
					player.AddBuff(mod.BuffType("CultistBuff4"), 1800);
				}

				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, projId, projectile.damage, projectile.knockBack, projectile.owner);
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 1, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndex].velocity *= 1.2f;
			}
		}
	}

	public class CultistPillar1 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1800;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.position.Y = player.Center.Y - 70;
			projectile.position.X = player.Center.X + 10;
		}
	}

	public class CultistPillar2 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1800;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.position.Y = player.Center.Y - 70;
			projectile.position.X = player.Center.X + 10;
		}
	}

	public class CultistPillar3 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1800;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.position.Y = player.Center.Y - 70;
			projectile.position.X = player.Center.X + 10;
		}
	}

	public class CultistPillar4 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1800;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.position.Y = player.Center.Y - 70;
			projectile.position.X = player.Center.X + 10;
		}
	}
}

namespace Monstroriam.Buffs
{
	public class CultistBuff1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Solar Buff");
			Description.SetDefault("+15 Defense");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 15;
		}
	}

	public class CultistBuff2 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Vortex Buff");
			Description.SetDefault("+10% Critical Chance");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.rangedCrit += 10;
			player.magicCrit += 10;
			player.meleeCrit += 10;
		}
	}

	public class CultistBuff3 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Nebula Buff");
			Description.SetDefault("-15% Mana Cost");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.manaCost -= 0.15f;
		}
	}

	public class CultistBuff4 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Stardust Buff");
			Description.SetDefault("+2 Sentry slots");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxTurrets += 2;
		}
	}
}