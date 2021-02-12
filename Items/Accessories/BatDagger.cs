using Monstroriam;
using Monstroriam.Projectiles;
using Monstroriam.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Accessories
{
	public class BatDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat Dagger");
			Tooltip.SetDefault("Summons a [c/EC6767:Spectral Bat] that leech foes"
				+ "\nIncreases jump height");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.shoot = ProjectileType<Projectiles.BatPetProj>();
			item.buffType = BuffType<Buffs.BatPetBuff>();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) 
			{
				player.AddBuff(item.buffType, 60, true);
			}
		}
	}
}

namespace Monstroriam.Buffs
{
	public class BatPetBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Leech Bat");
			Description.SetDefault("Periodically leech life from enemies");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.jumpBoost = true;
			player.buffTime[buffIndex] = 1;
			player.GetModPlayer<MyPlayer>().batPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<Projectiles.BatPetProj>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ProjectileType<Projectiles.BatPetProj>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class BatPetProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat Pet");
			Main.projFrames[projectile.type] = 4;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.DD2PetGato);
			aiType = ProjectileID.DD2PetGato;
			projectile.aiStyle = 62;
			projectile.width = 30;
			projectile.height = 24;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 20, 20, 80);
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			if (++projectile.frameCounter >= 10)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}

			if (Main.GameUpdateCount % 360 == 0)
			{
				if (Utils.SentryFindTarget(projectile, out Vector2 target, out float distance))
				{
					Vector2 direction = target - projectile.Center;
					Vector2 speed = Vector2.Zero;
					speed = direction * (15f / distance);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, mod.ProjectileType("BatLeechProj"), 1, projectile.knockBack, projectile.owner);
				}
			}

			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.dead)
			{
				modPlayer.batPet = false;
			}
			if (modPlayer.batPet)
			{
				projectile.timeLeft = 2;
			}
		}
	}

	public class BatLeechProj : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Main.rand.Next(-10, 11) * .35f, Main.rand.Next(-10, 11) * .35f, mod.ProjectileType("BatLeechProj2"), projectile.damage, projectile.knockBack, projectile.owner);
		}
	}

	public class BatLeechProj2 : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.tileCollide = false;
			projectile.penetrate = 5;
			projectile.timeLeft = 920;
			projectile.aiStyle = 3;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return false;
		}

		public override void AI()
		{
			Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
			Dust dust = Dust.NewDustPerfect(dustPosition, 64, null, 100, new Color(255, 75, 0), 1.4f);
			dust.velocity *= 0.3f;
			dust.noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			player.statLife += 1;
			if (Main.myPlayer == player.whoAmI)
			{
				player.HealEffect(1, true);
			}
		}
	}
}