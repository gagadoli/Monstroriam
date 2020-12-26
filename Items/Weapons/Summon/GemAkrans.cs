using Monstroriam;
using Monstroriam.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Items.Weapons.Summon
{
	public class GemAkrans : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Summons an army of [c/BF4E00:Gem] [c/636363:Squirrels]");
		}

		public override void SetDefaults() 
		{
			item.damage = 5;
			item.summon = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useStyle = 1;
			item.knockBack = 8;
			item.mana = 20;
			item.width = 62;
			item.height = 42;
			item.useTime = 50;
			item.useAnimation = 30;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("GemSquirrel1");
			item.shootSpeed = 7f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = Main.rand.Next(new int[] { type, ProjectileType<Projectiles.GemSquirrel2>(), ProjectileType<Projectiles.GemSquirrel3>(), ProjectileType<Projectiles.GemSquirrel4>(), ProjectileType<Projectiles.GemSquirrel5>(), ProjectileType<Projectiles.GemSquirrel6>(), ProjectileType<Projectiles.GemSquirrel7>() });
			int type2 = Main.rand.Next(new int[] { type, ProjectileType<Projectiles.GemSquirrel2>(), ProjectileType<Projectiles.GemSquirrel3>(), ProjectileType<Projectiles.GemSquirrel4>(), ProjectileType<Projectiles.GemSquirrel5>(), ProjectileType<Projectiles.GemSquirrel6>(), ProjectileType<Projectiles.GemSquirrel7>() });
			int type3 = Main.rand.Next(new int[] { type, ProjectileType<Projectiles.GemSquirrel2>(), ProjectileType<Projectiles.GemSquirrel3>(), ProjectileType<Projectiles.GemSquirrel4>(), ProjectileType<Projectiles.GemSquirrel5>(), ProjectileType<Projectiles.GemSquirrel6>(), ProjectileType<Projectiles.GemSquirrel7>() });
			
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;

			Projectile.NewProjectile(position.X, position.Y, speedX + 1, speedY, type2, damage, knockBack, player.whoAmI);

			Projectile.NewProjectile(position.X, position.Y, speedX - 1, speedY, type3, damage, knockBack, player.whoAmI);

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire);
			recipe.AddIngredient(ItemID.Ruby);
			recipe.AddIngredient(ItemID.Emerald);
			recipe.AddIngredient(ItemID.Topaz);
			recipe.AddIngredient(ItemID.Amethyst);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.Amber);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class GemSquirrel1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.10f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(233, 115, 255), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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

	public class GemSquirrel2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.12f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(35, 183, 216), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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

	public class GemSquirrel3 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.14f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(241, 159, 0), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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

	public class GemSquirrel4 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.16f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(33, 184, 115), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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

	public class GemSquirrel5 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.18f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(246, 99, 102), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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

	public class GemSquirrel6 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.20f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(229, 235, 242), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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

	public class GemSquirrel7 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1200;
			projectile.aiStyle = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextFloat() < 0.22f)
				{
					Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 16, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(252, 167, 45), 0.8f);
				}
			}

			if (++projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
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