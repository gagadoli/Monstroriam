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
	public class WarBanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/AC343C:War] [c/EDB361:Banner] to boost your melee/movement speed by 40%"
				+ "\n<right> to remove all sentries");
		}

		public override void SetDefaults()
		{
            item.sentry = true;
            item.noMelee = true;
            item.useStyle = 1;
			item.mana = 35;
			item.width = 24;
			item.height = 66;
			item.useTime = 25;
			item.useAnimation = 25;
            item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("WarBannerProj");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 35);
			recipe.AddIngredient(ItemID.WarTableBanner);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 worldStartPosition = new Vector2()
			{
				X = Main.mouseX + Main.screenPosition.X,
				Y = Main.mouseY + Main.screenPosition.Y
			};

			int yOffset = 15;

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
	public class BannerBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Banner Buff");
			Description.SetDefault("Faster melee");
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.meleeSpeed += 0.40f;
			player.moveSpeed += 0.40f;
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class WarBannerProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 66;
			projectile.friendly = true;
			projectile.sentry = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 3600;
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

			if (Main.rand.NextBool(2))
			{
				if (Main.rand.NextFloat() < 0.1f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, 0f, 0f);
				}
			}

			if (Main.GameUpdateCount % 100 == 0)
			{
				Player player = Main.player[projectile.owner];
				player.AddBuff(mod.BuffType("BannerBuff"), 200);
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 5, 0f, 0f, 100);
				Main.dust[dustIndex].noGravity = true;
			}
		}
	}
}