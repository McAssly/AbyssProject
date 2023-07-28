﻿using Abyss.Draw;
using Abyss.Entities.Magic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.Master
{
    internal class Effect
    {
        public Vector2 position;
        public AnimatedSprite sprite;
        private protected double lifetime;


        public Effect(Vector2 position, AnimatedSprite sprite, double lifetime)
        {
            this.position = position;
            this.sprite = sprite;
            this.lifetime = lifetime;
        }

        public void Update(double delta)
        {
            sprite.Update(delta);
            lifetime -= delta;
        }

        public bool IsDead()
        {
            return lifetime <= 0;
        }

        public static void BurstEffect(Vector2 position, GameMaster game_state)
        {
            game_state.effects.Add(new Effect(position, Globals.BurstEffect, 0.28));
        }
    }
}
