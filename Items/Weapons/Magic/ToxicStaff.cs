using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Monstroriam.Items.Weapons.Magic
{
	public class ToxicStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Staff");
			Tooltip.SetDefault("Shoots toxic bubbles");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 16;
			item.magic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 5;
            item.mana = 8;
			item.width = 46;
			item.height = 46;
			item.useTime = 30;
			item.useAnimation = 60;
            item.value = Item.buyPrice(0, 0, 10, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = ProjectileID.ToxicBubble;
			item.shootSpeed = 8f;
		}
	}
}