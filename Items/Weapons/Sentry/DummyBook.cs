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
	public class DummyBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a [c/D8A768:Dummy] to confuse foes" 
				+ "\n<right> to remove all sentries");
		}

		public override void SetDefaults()
		{
			item.damage = 4;
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
            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item44;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("DummyProj");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Book);
			recipe.AddIngredient(ItemID.TargetDummy);
			recipe.AddTile(TileID.Bookcases);
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
	public class DummyProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 9;
			ProjectileID.Sets.TurretFeature[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 50;
			projectile.friendly = true;
			projectile.sentry = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 8600;
		}

		public override bool? CanCutTiles()
		{
			return true;
		}

		public override bool? CanHitNPC(NPC npc)
		{
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Confused, 200);
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
				if (++projectile.frame >= 9)
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
		}
	}
}