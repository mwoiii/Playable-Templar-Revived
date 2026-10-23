using R2API;
using RoR2;
using RoR2BepInExPack.GameAssetPathsBetter;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Templar {
    public static class TemplarSkins {
        private const string altSkinNameToken = "PLAYABLETEMPLAR_ALT_SKIN";

        public static void RegisterSkins() {
            GameObject bodyPrefab = Templar.myCharacter;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            ModelSkinController skinController = model.GetComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();
            SkinnedMeshRenderer[] meshRenderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
            GameObject headMesh = meshRenderers.FirstOrDefault(x => x.name == "ClayBruiserHeadMesh").gameObject;

            #region Default
            SkinDefParams defaultSkinDefParams = new SkinDefParams() {
                rendererInfos = new CharacterModel.RendererInfo[meshRenderers.Length],
                meshReplacements = new SkinDefParams.MeshReplacement[meshRenderers.Length],
                gameObjectActivations = [
                    new SkinDefParams.GameObjectActivation() {
                        shouldActivate = true,
                        gameObject = headMesh
                    }
                ],
                projectileGhostReplacements = [],
                minionSkinReplacements = [],
                lightReplacements = [],
            };
            var templarSkinParams = Addressables.LoadAssetAsync<SkinDefParams>(RoR2_Base_ClayBruiser.skinClayBruiserBodyDefault_params_asset).WaitForCompletion();
            for (int i = 0; i < meshRenderers.Length; i++) {
                defaultSkinDefParams.rendererInfos[i].renderer = meshRenderers[i];
                defaultSkinDefParams.rendererInfos[i].defaultMaterialAddress = templarSkinParams.rendererInfos.FirstOrDefault(x => x.renderer?.name == meshRenderers[i].name).defaultMaterialAddress;
                defaultSkinDefParams.rendererInfos[i].defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                defaultSkinDefParams.rendererInfos[i].ignoreOverlays = false;
                defaultSkinDefParams.rendererInfos[i].hideOnDeath = false;
                defaultSkinDefParams.rendererInfos[i].ignoresMaterialOverrides = false;

                defaultSkinDefParams.meshReplacements[i].renderer = meshRenderers[i];
                defaultSkinDefParams.meshReplacements[i].meshAddress = templarSkinParams.meshReplacements.FirstOrDefault(x => x.renderer?.name == meshRenderers[i].name).meshAddress;
            }

            SkinDef defaultSkinDef = new SkinDef() {
                rootObject = characterModel.gameObject,
                nameToken = "DEFAULT_SKIN",
                icon = Skins.CreateSkinIcon(new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f), new Color(0.5f, 0.2f, 0f), new Color(0.5f, 0f, 0f)),
                skinDefParams = defaultSkinDefParams,
            };
            #endregion

            #region Alt
            SkinDef altSkinDef = UnityEngine.Object.Instantiate(defaultSkinDef);
            altSkinDef.icon = Skins.CreateSkinIcon(new Color(0.64f, 0.31f, 0.22f), new Color(0.48f, 0.34f, 0.09f), new Color(0.5f, 0.2f, 0f), new Color(0.5f, 0f, 0f));
            SkinDefParams altSkinDefParams = UnityEngine.Object.Instantiate(defaultSkinDefParams);
            altSkinDef.skinDefParams = altSkinDefParams;
            LanguageAPI.Add(altSkinNameToken, "Vagabond");
            altSkinDef.nameToken = altSkinNameToken;
            altSkinDef.name = "PlayableTemplarAltSkin";

            var potRenderer = Addressables.LoadAssetAsync<GameObject>(RoR2_Junk_Pot2.Pot2Body_prefab).WaitForCompletion().GetComponentInChildren<MeshRenderer>();
            GameObject potHeadPrefab = PrefabAPI.InstantiateClone(potRenderer.gameObject, "VagabondHead", false);
            UnityEngine.Object.Destroy(potHeadPrefab.GetComponent<HurtBoxGroup>());
            UnityEngine.Object.Destroy(potHeadPrefab.transform.GetComponentInChildren<HurtBox>());
            UnityEngine.Object.Destroy(potHeadPrefab.transform.GetChild(0).gameObject);

            altSkinDefParams.gameObjectActivations = [
                new SkinDefParams.GameObjectActivation() {
                    shouldActivate = true,
                    spawnPrefabOnModelObject = true,
                    gameObject = childLocator.FindChild("Head").gameObject,
                    prefab = potHeadPrefab,
                    localPosition = new Vector3(0f, -0.24f, 0f),
                    localRotation = new Vector3(-90f, 0f, 0f),
                    localScale = new Vector3(1f, 1f, 1.25f),
                },
                new SkinDefParams.GameObjectActivation() {
                    shouldActivate = false,
                    gameObject = headMesh
                }
            ];
            #endregion

            skinController.skins = [
                defaultSkinDef,
                altSkinDef
            ];
        }
    }
}