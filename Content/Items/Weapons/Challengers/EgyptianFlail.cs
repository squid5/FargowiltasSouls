using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Projectiles.ChallengerItems;

namespace FargowiltasSouls.Content.Items.Weapons.Challengers
{
    public class EgyptianFlail : SoulsItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<EgyptianFlailProjectile>(), 19, 2, 4);
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 2);
        }

        public int maxCooldown = 60 * 7;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (Main.myPlayer == player.whoAmI) {
                for (float i = -3.6f; i <= 3.6f; i += 3.6f)
                {
                    Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<EgyptianFlailProjectile>(), 16, 2, player.whoAmI, i, modPlayer.EgyptianFlailCD);
                }
            }
            if (modPlayer.EgyptianFlailCD <= 0)
            {
                modPlayer.EgyptianFlailCD = maxCooldown;
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (modPlayer.EgyptianFlailCD <= 0 && modPlayer.EgyptianFlailCD % 3 == 0)
            {
                Dust.NewDust(player.Center, 2, 2, DustID.Shadowflame);
            }
        }

        public override bool MeleePrefix()
        {
            return true;
        }

        public override void AddRecipes()
        {
            //CreateRecipe().AddIngredient<CursedCoffinBag>(2).AddTile(TileID.Solidifier).DisableDecraft().Register();
        }
    }
}
