using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace FargowiltasSouls.Content.Tiles
{
    public class CrackedSandstoneBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
            MinPick = 0;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][TileID.SandstoneBrick] = true;
            Main.tileMerge[TileID.SandstoneBrick][Type] = true;
            AddMapEntry(new(190f / 255, 171f / 255, 94f / 255));
        }
    }
}
