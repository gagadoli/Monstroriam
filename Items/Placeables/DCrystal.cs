using Terraria.ModLoader;

namespace Monstroriam.Items.Placeables
{
	public class DCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Orb");
			Tooltip.SetDefault("This thing should be put in a wall");
		}

		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 38;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 0;
			item.createTile = mod.TileType("DCrystal");
			item.placeStyle = 0;
		}
	}
}
