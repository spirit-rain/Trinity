using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Trinity.Projectiles.Melee
{
    public class SoulbladeProjectile : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 56;
			Projectile.height = 56; 
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = 999;

			AIType = ProjectileID.Bullet;
		}

		bool hasLaunched = false;
		int distance = 10;
		float ProjectileAngleDegrees;
		public override void AI()
        {
			if (Main.player[Projectile.owner].channel && !hasLaunched)
			{
				Projectile.timeLeft = 300;

				if (distance < 500)
					distance += 1;

				Projectile.rotation += (float)Math.Sqrt(distance) / 50;

				Projectile.ai[1]--;

				float angle = ProjectileAngleDegrees * (float)(Math.PI / 180);
				Projectile.position.X = Main.player[Projectile.owner].position.X + ((float)Math.Cos(angle) * distance / 3) - (Main.player[Projectile.owner].Size.X + Projectile.Size.X) / 4;
				Projectile.position.Y = Main.player[Projectile.owner].position.Y + ((float)Math.Sin(angle) * distance / 3);

				ProjectileAngleDegrees += (float)Math.Sqrt(distance) / 2;
				ProjectileAngleDegrees %= 360;
			}
			else
			{
				if (Projectile.velocity == Vector2.Zero)
				{
					Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Zero, (Main.MouseWorld - Projectile.position) * 2, 0.05f)) * (float)Math.Sqrt(distance) * 1.5f;
					Projectile.rotation = (Projectile.Center - Main.MouseWorld).ToRotation();
					Projectile.rotation += MathHelper.ToRadians(225);
					Projectile.damage += (int)Math.Sqrt(distance);
					TrinityPlayer.ModPlayer(Main.player[Projectile.owner]).circleProjCount--;
					distance = distance < 250 ? 250 : distance + 125;
				}

				Projectile.velocity *= 1.02f;
				hasLaunched = true;
			}
        }

		public override bool PreDraw(ref Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (distance > 250)
			{
				damage *= distance;
				damage /= 500;
			}
			else
				damage /= 2;

			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}