using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class CrystalWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Wand");
			Tooltip.SetDefault("Crystals falls from the sky");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 2;
            item.mana = 6;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;			
            item.value = Item.buyPrice(0, 12, 0, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileID.CrystalStorm;
			item.shootSpeed = 20f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 2 + Main.rand.Next(1);
			for (int index = 0; index < numberProjectiles; ++index)
			{
				Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(100) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //This defines the projectile width, direction and position.
				vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-100, 100);
				vector2_1.Y -= (float)(100 * index);
				float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
				float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
				if ((double)num13 < 0.0) num13 *= -1f;
				if ((double)num13 < 20.0) num13 = 20f;
				float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
				float num15 = item.shootSpeed / num14;
				float num16 = num12 * num15;
				float num17 = num13 * num15;
				float SpeedX = num16 + (float)Main.rand.Next(-12, 10) * 0.100f;  
				float SpeedY = num17 + (float)Main.rand.Next(-12, 10) * 0.100f;  
				Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, (float)Main.rand.Next(5));              
			}
			return false;
		}
	}
}