using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Trinity.Projectiles.Melee
{
    public class SoulReaverProjectile : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.scale = 1.5f;
			Projectile.width = 108;
			Projectile.height = 108;
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
		public float ProjectileAngleDegrees;
		float spin = 1f;
		int distance = 10;
		Vector2 followPos;
		Vector2 _velocity;
		public override void AI()
        {
			if (Main.player[Projectile.owner].channel && !hasLaunched)
			{
				Projectile.timeLeft = 300;

				Projectile.ai[1]--;
				if (distance < 1000)
					distance += 3;

				float angle = ProjectileAngleDegrees * (float)(Math.PI / 180);
				Projectile.position.X = Main.player[Projectile.owner].position.X + ((float)Math.Cos(angle) * distance / 3) - (Main.player[Projectile.owner].Size.X + Projectile.Size.X) / 4;
				Projectile.position.Y = Main.player[Projectile.owner].position.Y + ((float)Math.Sin(angle) * distance / 3) - Main.player[Projectile.owner].Size.Y;

				if (spin < 9f)
					spin += 0.02f;

				ProjectileAngleDegrees += spin;
				ProjectileAngleDegrees %= 360;
				Projectile.rotation = MathHelper.ToRadians(ProjectileAngleDegrees + 45);
			}
			else
			{
				if (!hasLaunched)
				{
					_velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Zero, (Main.MouseWorld - Main.player[Projectile.owner].position) * 2, 0.05f)) * (float)Math.Sqrt(distance) * 1.5f;
					followPos = Main.player[Projectile.owner].position;
					Projectile.damage += (int)Math.Sqrt(distance);
					TrinityPlayer.ModPlayer(Main.player[Projectile.owner]).circleProjCount = 0;
				}

				followPos += _velocity * 0.05f;
				_velocity *= 1.03f;
				hasLaunched = true;

				Projectile.ai[1]--;
				float angle = ProjectileAngleDegrees * (float)(Math.PI / 180);
				Projectile.position.X = followPos.X + ((float)Math.Cos(angle) * distance / 3) - (Main.player[Projectile.owner].Size.X + Projectile.Size.X) / 4;
				Projectile.position.Y = followPos.Y + ((float)Math.Sin(angle) * distance / 3) - Main.player[Projectile.owner].Size.Y;

				if (spin < 9f)
					spin += 0.02f;

				ProjectileAngleDegrees += spin;
				ProjectileAngleDegrees %= 360;
				Projectile.rotation = MathHelper.ToRadians(ProjectileAngleDegrees + 45);
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
			if (hasLaunched)
				damage = (int)(damage * 1.5f);

			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}
	}
}