using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Trinity.Projectiles.Melee
{
    public class TrueSoulbladeSpiritProjectile : ModProjectile
    {

		public override void SetDefaults()
		{
			Projectile.scale = 1.25f;
			Projectile.width = 62;
			Projectile.height = 62; 
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = 999;
			Projectile.light = 0.5f;

			AIType = ProjectileID.Bullet;
		}

		int lifetime = 90;
		public override void AI()
        {
			if (Projectile.alpha < 230 && lifetime == 0)
			{
				Projectile.alpha += 6;
				Projectile.timeLeft = 300;
				Projectile.velocity *= 0.992f;
			}
			else if (lifetime > 0)
				lifetime--;
			else
				Projectile.timeLeft = 0;

			Projectile.rotation += (float)Math.Sqrt(255 - Projectile.alpha) / 50;

			Projectile.ai[1]--;
		}

		public void SetData(Vector2 _velocity)
        {
			Projectile.velocity = _velocity;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(Color.White);
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (Projectile.alpha > 127)
			{
				damage *= 255 - Projectile.alpha;
				damage /= 255;
			}
			else
				damage /= 2;

			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}