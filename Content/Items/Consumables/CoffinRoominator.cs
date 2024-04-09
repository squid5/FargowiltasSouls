using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using FargowiltasSouls.Content.WorldGeneration;

namespace FargowiltasSouls.Content.Items.Consumables
{
    public class CoffinRoominator : SoulsItem
    {
        public override bool IsLoadingEnabled(Mod mod) => true;
        public override string Texture => "FargowiltasSouls/Content/Items/Placeholder";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Cyan;
            Item.maxStack = Item.CommonMaxStack;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.consumable = true;
            Item.UseSound = SoundID.Roar;
            Item.value = Item.sellPrice(0, 0, 0, 1);
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            CoffinArena.Place(Main.MouseWorld.ToTileCoordinates());
            return true;
        }
    }
}
