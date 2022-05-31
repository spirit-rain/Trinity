using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Trinity.Projectiles.Melee
{
    public class TrueSoulbladeProjectile : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

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

			AIType = ProjectileID.Bullet;
		}

		bool hasLaunched = false;
		int distance = 10;
		float ProjectileAngleDegrees;
		int launchTime = 0;
		int launchDelay = 60;
		public override void AI()
        {
			if (Main.player[Projectile.owner].channel && !hasLaunched)
			{
				Projectile.timeLeft = 300;

				if (distance < 800)
					distance += 2;

				Projectile.rotation += (float)Math.Sqrt(distance) / 50;

				Projectile.ai[1]--;

				float angle = ProjectileAngleDegrees * (float)(Math.PI / 180);
				Projectile.position.X = Main.player[Projectile.owner].position.X + ((float)Math.Cos(angle) * distance / 3) - (Main.player[Projectile.owner].Size.X + Projectile.Size.X) / 4;
				Projectile.position.Y = Main.player[Projectile.owner].position.Y + ((float)Math.Sin(angle) * distance / 3);

				ProjectileAngleDegrees += (float)Math.Sqrt(distance) / 2;
				ProjectileAngleDegrees %= 360;

				launchTime++;
				if (launchTime >= launchDelay)
                {
					launchTime = 0;
					if (launchDelay > 10)
						launchDelay -= 2;

					int projectileIndex = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<TrueSoulbladeSpiritProjectile>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner).identity;
					TrueSoulbladeSpiritProjectile trueSoulbladeSpiritProjectile = Main.projectile[projectileIndex].ModProjectile as TrueSoulbladeSpiritProjectile;

					trueSoulbladeSpiritProjectile.SetData(Vector2.Normalize(Vector2.Lerp(Vector2.Zero, (Main.MouseWorld - Projectile.position) * 2, 0.05f)) * (float)Math.Sqrt(distance) * 0.35f);
				}
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
					distance = distance < 400 ? 400 : distance + 200;
				}

				Projectile.velocity *= 1.03f;
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
			if (distance > 400)
			{
				damage *= distance;
				damage /= 800;
			}
			else
				damage /= 2;

			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}