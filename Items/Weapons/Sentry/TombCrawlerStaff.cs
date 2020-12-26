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
	public class TombCrawlerStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/D8A768:Tomb Crawler Statue] to create earthquakes" 
				+ "\n<right> to remove all sentries");
		}

		public override void SetDefaults()
		{
			item.damage = 12;
            item.summon = true;
            item.sentry = true;
            item.noMelee = true;
            item.useStyle = 1;
			item.knockBack = 6;
			item.mana = 20;
			item.width = 72;
			item.height = 72;
			item.useTime = 25;
			item.useAnimation = 25;
            item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("SmasherSentry");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sandstone, 35);
			recipe.AddIngredient(ItemID.AntlionMandible, 2);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{
				X = Main.mouseX + Main.screenPosition.X,
				Y = Main.mouseY + Main.screenPosition.Y
			};

			int yOffset = 1;

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
	public class SmasherSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 110;
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

			if (Main.GameUpdateCount % 100 == 0)
			{
				if (Utils.SentryFindTarget(projectile, out Vector2 target, out float distance))
				{
					Vector2 direction = target - projectile.Center;
					Vector2 speed = Vector2.Zero;
					speed = direction * (5f / distance);
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, mod.ProjectileType("SmashProj"), projectile.damage, projectile.knockBack, projectile.owner);
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
		}
	}

	public class SmashProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.DD2OgreSmash);
			aiType = ProjectileID.DD2OgreSmash;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.DD2OgreSmash; }
		}
	}
}