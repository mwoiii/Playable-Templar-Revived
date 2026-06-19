using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Templar {
    public class TemplarFireMiniRocket : BaseState {

        public static GameObject rocketPrefab;

        public static GameObject effectPrefab;

        public static int projectileCount = 1;

        public static float totalYawSpread = 1f;

        public static float baseDuration = 0.25f;

        public static float baseFireDuration = 0.25f;

        public static float damageCoefficient = Templar.miniBazookaDamageCoefficient.Value;

        public static float force = 25f;

        public static float recoilAmplitude = 7.5f;

        private float duration;

        private float fireDuration;

        private int projectilesFired;

        private bool jelly;

        public override void OnEnter() {
            base.OnEnter();
            StartAimMode(2f, false);
            jelly = false;
            rocketPrefab = Templar.templarRocket;
            effectPrefab = FireMegaFireball.muzzleflashEffectPrefab;
            AddRecoil(-1f * recoilAmplitude, -2f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);
            duration = baseDuration / attackSpeedStat;
            fireDuration = baseFireDuration / attackSpeedStat;
            Util.PlaySound(FireMegaFireball.attackString, gameObject);
        }

        public override void OnExit() {
            base.OnExit();
            PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
            GetModelAnimator().SetBool("WeaponIsReady", false);
            characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            string muzzleName = MinigunState.muzzleName;
            if (isAuthority) {
                int num = Mathf.FloorToInt(fixedAge / fireDuration * projectileCount);
                if (projectilesFired <= num && projectilesFired < projectileCount) {
                    EffectManager.SimpleMuzzleFlash(effectPrefab, gameObject, muzzleName, false);
                    Ray aimRay = GetAimRay();
                    float speedOverride = FireMegaFireball.projectileSpeed * 2f;
                    float bonusYaw = Mathf.FloorToInt(projectilesFired - (projectileCount - 1) / 2f) / (float)(TemplarFireMiniRocket.projectileCount - 1) * TemplarFireMiniRocket.totalYawSpread;
                    bonusYaw = 0f;
                    Vector3 forward = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, bonusYaw, 0f);
                    ProjectileManager.instance.FireProjectile(rocketPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(forward), base.gameObject, this.damageStat * TemplarFireMiniRocket.damageCoefficient, TemplarFireMiniRocket.force, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, speedOverride);
                    projectilesFired++;
                }
            }

            if (fixedAge >= duration && isAuthority) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
