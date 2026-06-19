using EntityStates;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using UnityEngine;

namespace Templar {
    public class TemplarChargeMiniRocket : BaseState {
        public override void OnEnter() {
            base.OnEnter();
            this.duration = TemplarChargeMiniRocket.baseDuration / this.attackSpeedStat;
            UnityEngine.Object modelAnimator = base.GetModelAnimator();
            Transform modelTransform = base.GetModelTransform();
            base.GetModelAnimator().SetBool("WeaponIsReady", true);
            base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
            base.characterBody._defaultCrosshairPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/crosshair/ToolbotGrenadeLauncherCrosshair");
            Util.PlayAttackSpeedSound(ChargeMegaFireball.attackString, base.gameObject, this.attackSpeedStat);
            if (modelTransform) {
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if (component) {
                    Transform transform = component.FindChild(MinigunState.muzzleName);
                    if (transform && ChargeMegaFireball.chargeEffectPrefab) {
                        this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMegaFireball.chargeEffectPrefab, transform.position, transform.rotation);
                        this.chargeInstance.transform.parent = transform;
                        ScaleParticleSystemDuration psd = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
                        if (psd) {
                            psd.newDuration = this.duration;
                        }
                    }
                }
            }
        }

        public override void OnExit() {
            base.OnExit();
            if (this.chargeInstance) {
                EntityState.Destroy(this.chargeInstance);
            }
        }

        public override void Update() {
            base.Update();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            base.StartAimMode(2f, false);
            if (base.fixedAge >= this.duration && base.isAuthority) {
                TemplarFireMiniRocket nextState = new TemplarFireMiniRocket();
                this.outer.SetNextState(nextState);
            }
        }

        protected ref InputBankTest.ButtonState skillButtonState {
            get {
                return ref base.inputBank.skill1;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }

        public static float baseDuration = 1.25f;

        private float duration;

        private GameObject chargeInstance;
    }
}
