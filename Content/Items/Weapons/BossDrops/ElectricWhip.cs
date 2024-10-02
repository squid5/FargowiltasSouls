using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Content.Projectiles.BossWeapons;

namespace FargowiltasSouls.Content.Items.Weapons.BossDrops
{
    public class ElectricWhip : SoulsItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<ElectricWhipProj>(), 60, 2, 10, 60);
            Item.rare = ItemRarityID.Pink;
            Item.value = 100000;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
