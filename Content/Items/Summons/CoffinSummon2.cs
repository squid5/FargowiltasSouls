
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using FargowiltasSouls.Content.Bosses.CursedCoffin;
using FargowiltasSouls.Content.WorldGeneration;
using Microsoft.Xna.Framework;
using static FargowiltasSouls.Core.Systems.WorldSavingSystem;
using System;
using Fargowiltas;
using FargowiltasSouls.Core.Systems;

namespace FargowiltasSouls.Content.Items.Summons
{

    public class CoffinSummon2 : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coffin Summon");
            //Tooltip.SetDefault("While in the underground Desert, summon the Cursed Coffin");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override bool IsLoadingEnabled(Mod mod) => CursedCoffin.Enabled;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = true;
            Item.maxStack = 20;
            Item.noUseGraphic = false;
        }
        Condition DownedCursedCoffin = new("Mods.FargowiltasSouls.Conditions.DownedCursedCoffin", () => DownedBoss[(int)Downed.CursedCoffin]);
        public override void AddRecipes()
        {
            CreateRecipe()
                //.AddRecipeGroup("FargowiltasSouls:AnyDemoniteBar", 4)
                .AddIngredient(ItemID.ClayBlock, 15)
                .AddIngredient(ItemID.FossilOre, 8)
                .AddIngredient(ItemID.Sapphire, 2)
                .AddCondition(DownedCursedCoffin)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override bool CanUseItem(Player player)
        {
            if (CoffinArena.Rectangle.Contains(player.Center.ToTileCoordinates()))// && (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight))
                return !NPC.AnyNPCs(NPCType<CursedCoffin>());
            return false;
        }

        public override bool? UseItem(Player player)
        {
            Vector2 coffinArenaCenter = CoffinArena.Center.ToWorldCoordinates();
            SoundEngine.PlaySound(CursedCoffin.ShotSFX with { Pitch = -0.75f }, coffinArenaCenter);
            int n = NPC.NewNPC(player.GetSource_ItemUse(Item), (int)coffinArenaCenter.X, (int)coffinArenaCenter.Y, ModContent.NPCType<CursedCoffin>());
            if (n.IsWithinBounds(Main.maxNPCs))
            {
                if (Main.npc[n].ModNPC is CursedCoffin coffin)
                    coffin.LockVector1 = coffinArenaCenter;
            }
            return true;
        }
    }
}
