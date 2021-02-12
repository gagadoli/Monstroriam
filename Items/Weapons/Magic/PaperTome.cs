using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monstroriam.Items.Weapons.Magic
{
	public class PaperTome : ModItem
	{
		public override void SetStaticDefaults() 
		{		
			Tooltip.SetDefault("Shoots [c/C4C4C4:Paper]");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults() 
		{
			item.damage = 8;
			item.magic = true;
			item.crit = 50;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 2;
			item.mana = 1;
			item.width = 46;
			item.height = 46;
			item.useTime = 12;
			item.useAnimation = 24;
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.rare = ItemRarityID.White;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PaperProj");
			item.shootSpeed = 6f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
	}
}

namespace Monstroriam.Projectiles
{
	public class PaperProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.BookStaffShot);
			aiType = ProjectileID.BookStaffShot;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 1;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.BookStaffShot; }
		}

		public override void AI()
		{
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 8)
				{
					projectile.frame = 0;
				}
			}
		}
	}
}