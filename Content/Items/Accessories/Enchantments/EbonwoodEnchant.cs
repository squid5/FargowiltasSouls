using FargowiltasSouls.Content.Buffs.Souls;
using FargowiltasSouls.Content.Projectiles;
using FargowiltasSouls.Core.AccessoryEffectSystem;
using FargowiltasSouls.Core.Toggler.Content;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static FargowiltasSouls.Content.Items.Accessories.Forces.TimberForce;

namespace FargowiltasSouls.Content.Items.Accessories.Enchantments
{
    public class EbonwoodEnchant : BaseEnchant
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override Color nameColor => new(100, 90, 141);


        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.rare = ItemRarityID.Blue;
            Item.value = 10000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddEffect<EbonwoodEffect>(Item);

        }

        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemID.EbonwoodHelmet)
            .AddIngredient(ItemID.EbonwoodBreastplate)
            .AddIngredient(ItemID.EbonwoodGreaves)
            .AddIngredient(ItemID.VileMushroom)
            .AddIngredient(ItemID.BlackCurrant)
            .AddIngredient(ItemID.LightlessChasms)


            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
    public class EbonwoodEffect : AccessoryEffect
    {
        public override Header ToggleHeader => Header.GetHeader<TimberHeader>();
        public override int ToggleItemType => ModContent.ItemType<EbonwoodEnchant>();

        public override void PostUpdateEquips(Player player)
        {
            FargoSoulsPlayer modPlayer = player.FargoSouls();
            if (player.whoAmI != Main.myPlayer)
                return;

            bool forceEffect = modPlayer.ForceEffect<EbonwoodEnchant>();
            int dist = ShadewoodEffect.Range(player, forceEffect);
            foreach (NPC npc in Main.npc.Where(n => n.active && !n.friendly && n.lifeMax > 5 && !n.dontTakeDamage))
            {
                Vector2 npcComparePoint = FargoSoulsUtil.ClosestPointInHitbox(npc, player.Center);
                if (player.Distance(npcComparePoint) < dist && (forceEffect || Collision.CanHitLine(player.Center, 0, 0, npcComparePoint, 0, 0)))
                {
                    if (!(npc.HasBuff<WitheredWizardBuff>() || npc.HasBuff<WitheredBuff>()))
                    {
                        npc.AddBuff(ModContent.BuffType<CorruptingBuff>(), 2);
                    }
                }
                float corruptTime = player.HasEffect<TimberEffect>() ? 10f : 60 * 3;
                if (npc.FargoSouls().EbonCorruptionTimer > corruptTime && (!(npc.HasBuff<WitheredWizardBuff>() || npc.HasBuff<WitheredBuff>())))
                {
                    EbonwoodProc(player, npc, dist, forceEffect, 5);
                }
            }
            //dust
            if (!MoltenAuraProj.CombinedAura(player))
            {
                int visualProj = ModContent.ProjectileType<EbonwoodAuraProj>();
                if (player.ownedProjectileCounts[visualProj] <= 0)
                {
                    Projectile.NewProjectile(GetSource_EffectItem(player), player.Center, Vector2.Zero, visualProj, 0, 0, Main.myPlayer);
                }
            }
        }
        public static void EbonwoodProc(Player player, NPC npc, int AoE, bool force, int limit)
        {
            //corrupt all in vicinity
            foreach (NPC npcToProcOn in Main.npc.Where(n => n.active && !n.friendly && n.lifeMax > 5 && !n.dontTakeDamage))
            {
                Vector2 npcComparePoint = FargoSoulsUtil.ClosestPointInHitbox(npcToProcOn, npc.Center);
                if (npc.Distance(npcComparePoint) < AoE && !npc.HasBuff<WitheredWizardBuff>() && !npc.HasBuff<WitheredBuff>() && limit > 0)
                {
                    EbonwoodProc(player, npc, AoE, force, limit - 1); //yes this chains (up to 3 times deep)
                }
            }

            Corrupt(npc, force);
            SoundEngine.PlaySound(SoundID.NPCDeath55, npc.Center);
            npc.FargoSouls().EbonCorruptionTimer = 0;

            //dust
            for (int i = 0; i < 60; i++)
            {
                Vector2 offset = new();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * AoE);
                offset.Y += (float)(Math.Cos(angle) * AoE);
                Vector2 spawnPos = npc.Center + offset - new Vector2(4, 4);
                Dust dust = Main.dust[Dust.NewDust(spawnPos, 0, 0, DustID.Shadowflame, 0, 0, 100, Color.White, 1f)];
                dust.velocity = npc.velocity;
                if (Main.rand.NextBool(3))
                    dust.velocity += Vector2.Normalize(offset) * -5f;
                dust.noGravity = true;
            }
        }
        private static void Corrupt(NPC npc, bool force)
        {
            if (npc.HasBuff<WitheredWizardBuff>() || npc.HasBuff<WitheredBuff>()) //don't stack the buffs under any circumstances
            {
                return;
            }
            if (force)
            {
                npc.AddBuff(ModContent.BuffType<WitheredWizardBuff>(), 60 * 4);
                return;
            }
            npc.AddBuff(ModContent.BuffType<WitheredBuff>(), 60 * 4);
        }
    }
}
