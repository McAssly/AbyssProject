
using Abyss.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Abyss.Entities
{
    internal enum Element
    {
        NULL = 0, 
    }

    internal class Particle
    {
        private protected Entity parent;
        public Vector2 position;
        public Vector2 velocity;
        public double accel;
        public double lifetime;
        public Element element;
        public double damage;

        public Particle(Entity parent, Vector2 position, Vector2 velocity, double accel, double lifetime, Element element, double damage)
        {
            this.parent = parent;
            this.position = position;
            this.velocity = velocity;
            this.accel = accel;
            this.lifetime = lifetime;
            this.element = element;
            this.damage = damage;
        }

        public void Update()
        {

        }
        public void Draw()
        {

        }
    }

    internal class Grimoire
    {
        public List<Particle> Particles = new List<Particle>();
        private double accel = 0;
        private double lifetime = 1.0;
        private Element element = Element.NULL;
        private double base_damage = 1;
        private double base_speed = 1;

        public void Attack(Entity parent, Vector2 targetPos)
        {
            Particles.Add(new Particle(
                parent, parent.Position(), 
                Vector2.Subtract(parent.Position(), MathUtil.MoveToward(parent.Position(), targetPos, base_speed)),
                accel, lifetime, element, parent.CalculateDamage(base_damage)
                ));
        }

        public void Update()
        {

        }
        public void Draw()
        {


        }

        public static Grimoire Which(string grimoire)
        {
            switch (grimoire)
            {
                case "base":
                    return new Grimoire();
                case "water":
                    return new WaterGrimoire();
                case "wind":
                    return new WindGrimoire();
                case "earth":
                    return new EarthGrimoire();
                case "fire":
                    return new FireGrimoire();
                case "lightning":
                    return new LightningGrimoire();
                default: return new Grimoire();
            }
        }

        public override string ToString()
        {
            return "base";
        }
    }

    internal class WaterGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "water";
        }
    }

    internal class WindGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "wind";
        }
    }

    internal class EarthGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "earth";
        }
    }

    internal class FireGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "fire";
        }
    }

    internal class LightningGrimoire : Grimoire
    {
        public override string ToString()
        {
            return "lightning";
        }
    }

}
