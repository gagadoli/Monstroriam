using Terraria.ModLoader;

namespace Monstroriam.Items.Placeables
{
	public class LeafOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leaf Orb");
			Tooltip.SetDefault("Made from the essence of a dryad");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 10000;
			item.createTile = mod.TileType("LeafOrb");
			item.placeStyle = 0;
		}
	}
}
