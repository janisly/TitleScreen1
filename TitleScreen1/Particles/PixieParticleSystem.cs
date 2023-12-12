using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flycatcher.Particles
{
    public class PixieParticleSystem : ParticleSystem
    {
        IParticleEmitter _emitter;

        public PixieParticleSystem(Game game, IParticleEmitter emitter) : base(game, 2000)
        {
            _emitter = emitter;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "particle";

            minNumParticles = 2;
            maxNumParticles = 5;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            var Velocity = _emitter.Velocity;

            var Acceleration = Vector2.UnitY * 400;

            var scale = RandomHelper.NextFloat(0.1f, 0.5f);

            var lifetime = RandomHelper.NextFloat(0.1f, 1.0f);

            var offsetX = RandomHelper.NextFloat(-20.0f, 20.0f);
            var offsetY = RandomHelper.NextFloat(-10.0f, 10.0f);

            var offsetVector = new Vector2(offsetX, offsetY);

            p.Initialize(where + offsetVector, Velocity, Acceleration, p.Color, scale: scale, lifetime: lifetime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            AddParticles(_emitter.Position);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            float alpha = 8 * normalizedLifetime * (1 - normalizedLifetime);
            particle.Color = Color.Red * alpha;

            particle.Scale = .2f + .25f * normalizedLifetime / 2;
        }
    }
}
