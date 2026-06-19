using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace Templar {
    public static class Projectiles {
        internal static void ProjectileSetup() {
            Templar.templarGrenade = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/CommandoGrenadeProjectile").InstantiateClone("TemplarGrenadeProjectile", true);
            Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 4f;
            Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().blastProcCoefficient = 0.8f;
            Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().blastRadius = 12f;
            Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().falloffModel = BlastAttack.FalloffModel.Linear;
            Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().lifetimeAfterImpact = 0.15f;
            Templar.templarGrenade.GetComponent<ProjectileImpactExplosion>().impactEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosivePotExplosion");
            Templar.templarGrenade.GetComponent<ProjectileDamage>().damageType = DamageType.ClayGoo;
            GameObject gameObject = Assets.clayBombModel.InstantiateClone("TemplarBombModel", true);
            gameObject.AddComponent<ProjectileGhostController>();
            gameObject.transform.GetChild(0).localScale *= 0.5f;
            Templar.templarGrenade.GetComponent<ProjectileController>().ghostPrefab = gameObject;
            gameObject.AddComponent<NetworkIdentity>();
            //gameObject.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Models/TemplarBombModel", 500);
            //Templar.templarGrenade.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Projectiles/TemplarGrenadeProjectile", 48);


            Templar.templarRocket = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LemurianBigFireball").InstantiateClone("TemplarRocketProjectile", true);

            if (Templar.templarRocket.TryGetComponent(out ProjectileImpactExplosion impactExplosion)) {
                impactExplosion.blastDamageCoefficient = 1f;
                impactExplosion.blastRadius = 16f;
                impactExplosion.blastProcCoefficient = 0.8f;
                impactExplosion.destroyOnEnemy = true;
                impactExplosion.destroyOnWorld = true;
            }

            if (Templar.templarRocket.TryGetComponent(out MissileController missileController)) {
                UnityEngine.Object.Destroy(missileController);
            }

            Templar.templarRocket.AddComponent<TemplarMissileController>();
            Templar.templarRocket.AddComponent<TemplarExplosionForce>();
            GameObject missileModel = Assets.clayMissileModel.InstantiateClone("TemplarMissileModel", true);
            missileModel.AddComponent<ProjectileGhostController>();
            missileModel.transform.GetChild(1).SetParent(missileModel.transform.GetChild(0));
            missileModel.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 90f, 0f);
            missileModel.transform.GetChild(0).localScale *= 0.5f;
            missileModel.transform.GetChild(0).GetChild(0).GetChild(0).SetParent(missileModel.transform.GetChild(0));
            GameObject gameObject3 = missileModel.transform.GetChild(0).GetChild(0).gameObject;
            gameObject3.AddComponent<TemplarSeparateFromParent>();
            gameObject3.transform.localScale *= 0.8f;
            Templar.templarRocket.GetComponent<ProjectileController>().ghostPrefab = missileModel;
            Templar.templarRocketEffect = Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().impactEffect.InstantiateClone("TemplarRocketImpact", true);
            if (Templar.templarRocketEffect.TryGetComponent(out VFXAttributes vfxAttributes)) {
                vfxAttributes.vfxPriority = VFXAttributes.VFXPriority.Always;
            }
            R2API.ContentAddition.AddEffect(Templar.templarRocketEffect);
            Templar.templarRocket.GetComponent<ProjectileImpactExplosion>().impactEffect = Templar.templarRocketEffect;
            Templar.templarRocket.GetComponent<ProjectileDamage>().damageType = DamageType.BypassOneShotProtection;
            missileModel.AddComponent<NetworkIdentity>();
            //gameObject2.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Models/TemplarMissileModel", 501);
            //Templar.templarRocket.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/Projectiles/TemplarRocketProjectile", 44);
            Templar.templarRocketEffect.AddComponent<NetworkIdentity>();
            //Templar.templarRocketEffect.RegisterNetworkPrefab("C:Lemurian.cs", "Prefabs/effects/TemplarRocketImpact", 45);
            Loader.projectilePrefabs.Add(Templar.templarGrenade);
            Loader.projectilePrefabs.Add(Templar.templarRocket);
        }
    }
}
