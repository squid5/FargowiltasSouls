using FargowiltasSouls.Common.Graphics.Particles;
using FargowiltasSouls.Content.Items.Accessories.Enchantments;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Systems;
using FargowiltasSouls.Core.Toggler.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Content.Items.Accessories.Forces
{
	public class ShadowForce : BaseForce
    {
        public override void SetStaticDefaults()
        {
            Enchants[Type] = new int[]
            {
                ModContent.ItemType<NinjaEnchant>(),
                ModContent.ItemType<AncientShadowEnchant>(),
                ModContent.ItemType<CrystalAssassinEnchant>(),
                ModContent.ItemType<SpookyEnchant>(),
                ModContent.ItemType<ShinobiEnchant>(),
                ModContent.ItemType<DarkArtistEnchant>(),
                ModContent.ItemType<NecroEnchant>()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SetActive(player);
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            player.AddEffect<NinjaEffect>(Item);
            modPlayer.ApprenticeEnchantActive = true;
            modPlayer.DarkArtistEnchantActive = true;
            player.AddEffect<ApprenticeSupport>(Item);
            player.AddEffect<DarkArtistMinion>(Item);
            player.AddEffect<NecroEffect>(Item);
            //shadow orbs
            modPlayer.AncientShadowEnchantActive = true;
            player.AddEffect<ShadowBalls>(Item);
            //darkness debuff
            player.AddEffect<AncientShadowDarkness>(Item);
            //shinobi and monk effects
            ShinobiEnchant.AddEffects(player, Item);
            //smoke bomb nonsense
            CrystalAssassinEnchant.AddEffects(player, Item);
            //scythe doom
            player.AddEffect<SpookyEffect>(Item);
            player.AddEffect<ShadowForceDashEffect>(Item);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            foreach (int ench in Enchants[Type])
                recipe.AddIngredient(ench);
            recipe.AddTile(ModContent.Find<ModTile>("Fargowiltas", "CrucibleCosmosSheet"));
            recipe.Register();
        }
    }
    public class ShadowForceDashEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<ShadowHeader>();
        public override int ToggleItemType => ModContent.ItemType<ShadowForce>();
        
        public override void PostUpdateEquips(Player player)
        {
            FargoSoulsPlayer farg = player.FargoSouls();
            if (farg.ShadowDashTimer > 0)
            {
                farg.ShadowDashTimer--;
                Particle part = new AlphaSparkParticle(player.oldPosition + player.Size*0.5f, player.velocity * 0.4f, Color.Black, 1, 10, false);
                part.Spawn();
                Particle bub = new AlphaExpandingBloomParticle(player.Center, -(player.velocity * 0.01f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30, 30))) , Color.Black, Vector2.One, Vector2.One*0.2f, 20, true);
                bub.Spawn();
            }
        }
        public static void AddDash(Player player)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (modPlayer.HasDash)
                return;
            modPlayer.HasDash = true;
            modPlayer.FargoDash = DashManager.DashType.Shadow;
        }
        public static void ShadowDash(Player player, int dir)
        {
            player.FargoSouls().ShadowDashTimer = 30;
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            float dashSpeed = 20f;
            player.velocity.X = dashSpeed * dir;
            if (modPlayer.IsDashingTimer < 20)
                modPlayer.IsDashingTimer = 20;
            player.dashDelay = 60;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(MessageID.PlayerControls, number: player.whoAmI);
        }
    }
}
