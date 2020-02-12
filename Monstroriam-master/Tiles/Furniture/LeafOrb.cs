using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Monstroriam.Tiles.Furniture
{
	public class LeafOrb : ModTile
	{
	    public override void SetDefaults()
	    {
		    Main.tileFrameImportant[Type] = true;
	        Main.tileLavaDeath[Type] = true;
		    TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
	        TileObjectData.newTile.StyleHorizontal = true;
		    TileObjectData.newTile.StyleWrapLimit = 36;
	        TileObjectData.addTile(Type);
			dustType = 2;
	    }

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
	        if(frameX == 0)
		    {
				Item.NewItem(i * 16, j * 16, 48, 48, ItemType("LeafOrb"));
			}
		}

		public override void NearbyEffects(int i, int j, bool closer)
	    {
		   if(closer)
		   {	
				Main.player[Main.myPlayer].AddBuff(BuffID.DryadsWard, 60);
	       }
		}
	}
}