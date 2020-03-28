using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Summon
{
	public class VulcanoWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulcano Wand");
			Tooltip.SetDefault("Summons a [c/FF5605:flying Vulcano] to shoot at your enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 19;
            item.summon = true;
            item.sentry = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.useStyle = 1;
            item.mana = 30;
			item.width = 44;
			item.height = 44;
			item.useTime = 25;
			item.useAnimation = 25;		
            item.value = Item.buyPrice(0, 0, 20, 40);
			item.rare = 2;
			item.UseSound = SoundID.Item44;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("VulcanoSentry");
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 SPos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			position = SPos;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && proj.owner == player.whoAmI)
				{
					proj.active = false;
				}
			}
			return player.altFunctionUse != 2;
		}
	}
}
