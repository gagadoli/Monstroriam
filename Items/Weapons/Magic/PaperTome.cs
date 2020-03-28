using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class PaperTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Paper Tome");
			Tooltip.SetDefault("Much more deadly than it looks");
			Item.staff[item.type] = true; 
		}

		public override void SetDefaults()
		{
			item.damage = 9;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 5;
            item.mana = 6;
			item.width = 28;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
            item.value = Item.buyPrice(0, 0, 30, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileID.BookStaffShot;
			item.shootSpeed = 15f;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
	}
}