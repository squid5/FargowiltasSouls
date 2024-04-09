using FargowiltasSouls.Core.Systems;
using Microsoft.Xna.Framework;
using StructureHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace FargowiltasSouls.Content.WorldGeneration
{
    public static class CoffinArena
    {
        public const int Width = 60;

        private const int PaddedWidth = Width + 2;

        public const int Height = 35;

        public static Point Center => WorldSavingSystem.CoffinArenaCenter;
        public static Rectangle Rectangle = new();

        public static bool TileIsPyramid(Tile tile) => (tile.TileType == TileID.SandstoneBrick || tile.WallType == WallID.SandstoneBrick);

        public static bool CheckSpot(int x, int y)
        {
            for (int xOff = -PaddedWidth / 2; xOff < PaddedWidth / 2; xOff++)
            {
                for (int yOff = 0; yOff < Height; yOff++)
                {
                    Tile tile = Main.tile[x + xOff, y + yOff];
                    if (tile.WallType == WallID.SandstoneBrick && tile.TileType != TileID.SandstoneBrick) // invalid position if hallway
                        return false;
                }
            }
            return true;
        }

        public static void Generate(Point pyramidTop)
        {
            // Find bottom of pyramid
            int curYSearch = 0;
            while (++curYSearch < 300) // max bound
            {
                int xSearchWidth = curYSearch - (PaddedWidth / 2);
                if (xSearchWidth < 0) // too thin
                    continue;
                for (int curXSearch = -xSearchWidth; curXSearch <= xSearchWidth; curXSearch++)
                {
                    bool validSpot = CheckSpot(pyramidTop.X + curXSearch, pyramidTop.Y + curYSearch);
                    if (!validSpot) 
                        continue;
                    Point arenaTopCenter = new(pyramidTop.X + curXSearch, pyramidTop.Y + curYSearch);
                    Place(arenaTopCenter);
                    Place(arenaTopCenter);
                    return;
                }
            }
        }

        public static void Place(Point arenaTopCenter)
        {
            Point16 arenaTopLeft = new(arenaTopCenter.X - (Width / 2) + 1, arenaTopCenter.Y);
            StructureHelper.Generator.GenerateStructure("Content/WorldGeneration/CoffinArena", arenaTopLeft, ModLoader.GetMod("FargowiltasSouls"));
            foreach (int x in new List<int> { arenaTopLeft.X - 1, arenaTopLeft.X + Width - 2 })
            {
                for (int y = 0; y < Height; y++)
                {
                    Point point = new(x, arenaTopCenter.Y + y);
                    WorldGen.KillTile(point.X, point.Y);
                    WorldGen.KillWall(point.X, point.Y);
                    WorldGen.PlaceTile(point.X, point.Y, TileID.SandstoneBrick, mute: true, forced: true);
                }
            }
            WorldSavingSystem.CoffinArenaCenter = new(arenaTopCenter.X, arenaTopCenter.Y + Height / 2);
            Rectangle = new(arenaTopCenter.X - Width / 2, arenaTopCenter.Y, Width, Height);

            /*
            // Place arena
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Point point = new(arenaTopCenter.X - (Width / 2) + x, arenaTopCenter.Y + y);
                    WorldGen.KillTile(point.X, point.Y);
                    WorldGen.KillWall(point.X, point.Y);
                    if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1) // place blocks on border
                    {
                        WorldGen.PlaceTile(point.X, point.Y, TileID.SandstoneBrick, mute: true, forced: true);
                    }
                    else
                    {
                        WorldGen.PlaceWall(point.X, point.Y, WallID.SandstoneBrick, true);
                        WorldGen.ReplaceWall(point.X, point.Y, WallID.SandstoneBrick);
                    }
                }
            }
            WorldSavingSystem.CoffinArenaCenter = new(arenaTopCenter.X, arenaTopCenter.Y + Height / 2);
            Rectangle = new(arenaTopCenter.X - Width / 2, arenaTopCenter.Y, Width, Height);
            */
        }

        public static Vector2 ClampWithinArena(Vector2 vector, Entity entityToPadBasedOn)
        {
            Vector2 center = Center.ToWorldCoordinates();
            float xBound = (Width * 8) - entityToPadBasedOn.width * 0.6f;
            float yBound = (Height * 8) - entityToPadBasedOn.height * 0.6f;
            vector.X = Math.Clamp(vector.X, center.X - xBound, center.X + xBound);
            vector.Y = Math.Clamp(vector.Y, center.Y - yBound, center.Y + yBound);
            return vector;
        }
        public static List<Vector2> ArenaCorners(Entity entityToPadBasedOn)
        {
            Vector2 center = Center.ToWorldCoordinates();
            Vector2 xBound = Vector2.UnitX * ((Width * 8) - entityToPadBasedOn.width * 0.6f);
            Vector2 yBound = Vector2.UnitY * ((Height * 8) - entityToPadBasedOn.height * 0.6f);
            return new List<Vector2>() { center - xBound - yBound, center - xBound + yBound, center + xBound - yBound, center + xBound + yBound };
        }
        public static List<Vector2> TopArenaCorners(Entity entityToPadBasedOn)
        {
            Vector2 center = Center.ToWorldCoordinates();
            Vector2 xBound = Vector2.UnitX * ((Width * 8) - entityToPadBasedOn.width * 0.6f);
            Vector2 yBound = Vector2.UnitY * ((Height * 8) - entityToPadBasedOn.height * 0.6f);
            return new List<Vector2>() { center - xBound - yBound, center + xBound - yBound };
        }
    }
}
