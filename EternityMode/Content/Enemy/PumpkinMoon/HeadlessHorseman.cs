﻿using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.EternityMode.Net;
using FargowiltasSouls.EternityMode.Net.Strategies;
using FargowiltasSouls.EternityMode.NPCMatching;
using FargowiltasSouls.NPCs;
using FargowiltasSouls.Projectiles;
using FargowiltasSouls.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.EternityMode.Content.Enemy.PumpkinMoon
{
    public class HeadlessHorseman : EModeNPCBehaviour
    {
        public override NPCMatcher CreateMatcher() => new NPCMatcher().MatchType(NPCID.HeadlessHorseman);

        public int Counter;

        public override void AI(NPC npc)
        {
            base.AI(npc);

            if (++Counter > 360)
            {
                Counter = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient && npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) < 800)
                {
                    Vector2 vel = (Main.player[npc.target].Center - npc.Center) / 60f;
                    if (vel.Length() < 12f)
                        vel = Vector2.Normalize(vel) * 12f;
                    Projectile.NewProjectile(npc.GetSpawnSource_ForProjectile(), npc.Center, vel, ModContent.ProjectileType<HorsemansBlade>(),
                        npc.damage / 5, 0f, Main.myPlayer, npc.target);
                }
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            base.OnHitPlayer(npc, target, damage, crit);

            target.GetModPlayer<FargoSoulsPlayer>().AddBuffNoStack(BuffID.Cursed, 30);
            target.AddBuff(ModContent.BuffType<LivingWasteland>(), 600);
        }
    }
}
