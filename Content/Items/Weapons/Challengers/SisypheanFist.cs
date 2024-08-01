using FargowiltasSouls.Content.Projectiles.ChallengerItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Weapons.Challengers
{
    public class SisypheanFist : SoulsItem
    {
        private int delay = 0;
        private bool LastMouse = false;

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 30;
            Item.useAnimation = 20;
            Item.channel = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<SandstoneBoulder>();
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.knockBack = 10f;
        }

        public override void HoldItem(Player player)
        {
            if (LastMouse && !Main.mouseLeft && delay == 0 && CanUseItem(player))
            {
                delay = 60;
            }
            if (delay > 0)
            {
                delay--;
            }
            LastMouse = Main.mouseLeft;
            base.HoldItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            // Stupid grounded checks
            bool grounded = player.velocity.Y == 0 && !player.mount.Active && player.gravDir > 0 && player.grapCount == 0;

            Tile tile = Framing.GetTileSafely(player.Bottom);
            Tile tile2 = Framing.GetTileSafely(player.BottomLeft);
            Tile tile3 = Framing.GetTileSafely(player.BottomRight);
            bool notPlatforms = !isPlatform(tile.TileType) && !isPlatform(tile2.TileType) && !isPlatform(tile3.TileType);

            return grounded && notPlatforms && delay <= 0 && base.CanUseItem(player);

            static bool isPlatform(int tileType)
            {
                return tileType == TileID.Platforms || tileType == TileID.PlanterBox;
            }
        }
    }
}
