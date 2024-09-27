using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Essences
{
    public class ApprenticesEssence : BaseEssence
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override Color nameColor => new(255, 83, 255);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.18f;
            player.GetCritChance(DamageClass.Magic) += 8;
            player.statManaMax2 += 50;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WandofFrosting)
                .AddIngredient(ItemID.ZapinatorGray)
                .AddRecipeGroup("FargowiltasSouls:VilethornOrCrimsonRod")
                .AddIngredient(ItemID.WeatherPain)
                .AddIngredient(ItemID.WaterBolt)
                .AddIngredient(ItemID.Flamelash)
                .AddIngredient(ItemID.DemonScythe)
                .AddIngredient(ItemID.SorcererEmblem)
                .AddIngredient(ItemID.HallowedBar, 5)

                .AddTile(TileID.TinkerersWorkbench)
                .Register();

        }
    }
}
