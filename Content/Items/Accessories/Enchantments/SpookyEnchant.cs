using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    public class SpookyEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override Color nameColor => new(100, 78, 116);


        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Lime;
            Item.value = 250000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<SpookyEffect>(Item);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpookyHelmet)
            .AddIngredient(ItemID.SpookyBreastplate)
            .AddIngredient(ItemID.SpookyLeggings)
            .AddIngredient(ItemID.ButchersChainsaw)
            .AddIngredient(ItemID.DeathSickle)
            .AddIngredient(ItemID.RavenStaff)

            //psycho knife
            //eoc yoyo
            //dark harvest
            //.AddIngredient(ItemID.CursedSapling);
            //.AddIngredient(ItemID.EyeSpring);

            .AddTile(TileID.CrystalBall)
            .Register();
        }
    }
    public class SpookyEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<ShadowHeader>();
        public override int ToggleItemType => ModContent.ItemType<SpookyEnchant>();
        public override bool ExtraAttackEffect => true;
    }
}
