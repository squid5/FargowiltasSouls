using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Essences
{
    public class BarbariansEssence : BaseEssence
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override Color nameColor => new(255, 111, 6);

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.18f;
            player.GetAttackSpeed(DamageClass.Melee) += .1f;
            player.GetCritChance(DamageClass.Melee) += 8;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ZombieArm)
                .AddIngredient(ItemID.ChainKnife)
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.JungleYoyo)
                .AddIngredient(ItemID.CombatWrench)
                .AddIngredient(ItemID.Terragrim)
                .AddIngredient(ItemID.Trimarang)
                .AddIngredient(ItemID.WarriorEmblem)
                .AddIngredient(ItemID.HallowedBar, 5)

                .AddTile(TileID.TinkerersWorkbench)
                .Register();

        }
    }
}
