using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Souls
{
    //[AutoloadEquip(EquipType.Neck)]
    public class SnipersSoul : BaseSoul
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }


        public static readonly Color ItemColor = new(188, 253, 68);
        protected override Color? nameColor => ItemColor;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.FargoSouls().UniverseSoul)
                return;
            //reduce ammo consume
            player.FargoSouls().RangedSoul = true;
            player.GetDamage(DamageClass.Ranged) += 0.3f;
            player.GetCritChance(DamageClass.Ranged) += 15;

            //add new effects

        }

        public override void AddRecipes()
        {
            CreateRecipe()

            /*
hive pack*/
            .AddIngredient(null, "SharpshootersEssence")
            .AddIngredient(ItemID.MoltenQuiver)
            .AddIngredient(ItemID.StalkersQuiver)
            .AddIngredient(ItemID.ReconScope)

            .AddIngredient(ItemID.DartPistol)
            .AddIngredient(ItemID.Megashark)
            .AddIngredient(ItemID.PulseBow)
            .AddIngredient(ItemID.NailGun)
            .AddIngredient(ItemID.PiranhaGun)
            .AddIngredient(ItemID.SniperRifle)
            .AddIngredient(ItemID.Tsunami)
            .AddIngredient(ItemID.StakeLauncher)
            .AddIngredient(ItemID.ElfMelter)
            .AddIngredient(ItemID.Xenopopper)
            .AddIngredient(ItemID.Celeb2)

            .AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"))
            .Register();

        }
    }
}
