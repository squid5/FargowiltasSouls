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

        public const int Height = 35;

        public static Point Center => WorldSavingSystem.CoffinArenaCenter;
        public static Rectangle Rectangle = new();

        public static bool TileIsPyramid(Tile tile) => (tile.TileType == TileID.SandstoneBrick || tile.WallType == WallID.SandstoneBrick);

        public static bool CheckSpot(int x, int y)
        {
            Point point = new(x, y);
            for (int xOff = -Width / 2; xOff < Width / 2; xOff++)
            {
                if (!TileIsPyramid(Main.tile[point.X + xOff, point.Y - 1])) // fail if not pyramid above
                    return false;
                for (int yOff = 0; yOff < Height; yOff++)
                    if (TileIsPyramid(Main.tile[point.X + xOff, point.Y + yOff])) // fail if pyramid here; aka passage
                        return false;
            }
            return true;
        }

        public static void Generate(Point pyramidTop)
        {
            // Find bottom of pyramid
            int pyramidHeight = 0;
            while (pyramidHeight < 300) // max bound
            {
                bool foundBottom = false;
                pyramidHeight++;
                for (int x = -(int)(pyramidHeight / 4f); x < (int)(pyramidHeight / 4f); x++)
                {
                    if (!TileIsPyramid(Main.tile[pyramidTop.X + x, pyramidTop.Y + pyramidHeight]))
                    {
                        foundBottom = true;
                        break;
                    }
                }
                if (foundBottom)
                    break;
            }

            Point pyramidBottom = new(pyramidTop.X, pyramidTop.Y + pyramidHeight);

            // Find spot for arena, avoiding the passage
            int xOffset = 0;
            int attempt = 0;
            while (attempt < 100)
            {
                xOffset = WorldGen.genRand.Next(-pyramidHeight, pyramidHeight); // property of pyramid triangle; bottom width = 2 * height
                bool validSpot = CheckSpot(pyramidBottom.X + xOffset, pyramidBottom.Y);
                if (validSpot)
                    break;
                attempt++;
            }
            Point arenaTopCenter = new(pyramidBottom.X + xOffset, pyramidBottom.Y - 1);
            Place(arenaTopCenter);
        }

        public static void Place(Point arenaTopCenter)
        {
            Point16 arenaTopLeft = new(arenaTopCenter.X - (Width / 2) + 1, arenaTopCenter.Y);
            StructureHelper.Generator.GenerateStructure("Content/WorldGeneration/CoffinArena", arenaTopLeft, ModLoader.GetMod("FargowiltasSouls"));
            foreach (int x in new List<int> { arenaTopLeft.X, arenaTopLeft.X + Width })
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
