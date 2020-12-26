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
	public class SlimeHammer : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("Throws [c/FF0000:slime hammers]" +
				"\n50% chance to spawn a slime");
		}

		public override void SetDefaults()
		{
			item.damage = 5;
			item.summon = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useStyle = 1;
			item.knockBack = 2;
			item.mana = 8;
			item.width = 28;
			item.height = 30;
			item.useTime = 35;
			item.useAnimation = 35;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("SlimeHammerProj");
			item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RichMahoganyHammer);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.AddIngredient(ItemID.SlimeBlock, 2);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class SlimeHammerProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 720;
			projectile.aiStyle = 2;
		}

		public override void AI()
		{
			projectile.velocity.Y = projectile.velocity.Y + 0.2f;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.NextBool(2))
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 16f, Main.rand.Next(-10, 11) * .25f, Main.rand.Next(-10, -5) * .25f, mod.ProjectileType("SlimeProj"), projectile.damage, projectile.knockBack, projectile.owner);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
			}
			return false;
		}
	}

	public class SlimeProj : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.ignoreWater = false;
			projectile.penetrate = 5;
			projectile.timeLeft = 720;
			projectile.aiStyle = 63;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override void AI()
		{
			projectile.velocity.Y = projectile.velocity.Y + 0.2f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 55, 0f, 0f, 100, default(Color), 1f);
			}
		}
	}
}