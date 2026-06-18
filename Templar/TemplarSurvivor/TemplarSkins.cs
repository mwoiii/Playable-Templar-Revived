using R2API;
using RoR2;
using RoR2BepInExPack.GameAssetPaths.Version_1_39_0;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Templar {
    public static class TemplarSkins {
        private static SkinDef.GameObjectActivation[] getActivations(GameObject[] allObjects, params GameObject[] activatedObjects) {
            List<SkinDef.GameObjectActivation> GameObjectActivations = new List<SkinDef.GameObjectActivation>();

            for (int i = 0; i < allObjects.Length; i++) {

                bool activate = activatedObjects.Contains(allObjects[i]);

                GameObjectActivations.Add(new SkinDef.GameObjectActivation {
                    gameObject = allObjects[i],
                    shouldActivate = activate
                });
            }

            return GameObjectActivations.ToArray();
        }

        public static void RegisterSkins() {
            GameObject bodyPrefab = Templar.myCharacter;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();
            ModelSkinController skinController = model.GetComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();
            SkinnedMeshRenderer[] meshRenderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();

            SkinDefParams ogSkinDefParams = Addressables.LoadAssetAsync<SkinDefParams>(RoR2_Base_ClayBruiser.skinClayBruiserBodyDefault_params_asset).WaitForCompletion();

            #region Default Skin

            LanguageAPI.Add("Templar_DEFAULT_SKIN", "Default");
            SkinDef skinDef = skinController.skins[0];
            //skinDef.skinDefParams = ScriptableObject.CreateInstance<RoR2.SkinDefParams>();
            //skinDef.baseSkins = Array.Empty<SkinDef>();
            skinDef.icon = LoadoutAPI.CreateSkinIcon(new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f), new Color(0.64f, 0.31f, 0.22f), new Color(0.54f, 0.21f, 0.12f));
            //skinDef.unlockableDef = null;
            //skinDef.rootObject = model;

            //CharacterModel.RendererInfo[] ogRenderers = ogSkinDefParams.rendererInfos;
            //skinDef.skinDefParams.rendererInfos = new CharacterModel.RendererInfo[ogRenderers.Length];
            //ogRenderers.CopyTo(skinDef.skinDefParams.rendererInfos, 0);
            //for (int i = 0; i < ogRenderers.Length; i++) {
            //    skinDef.skinDefParams.rendererInfos[i].renderer = meshRenderers.Where(renderer => renderer.gameObject.name == skinDef.skinDefParams.rendererInfos[i].renderer.gameObject.name).First();
            //}

            //Mesh[] ogMeshes = new Mesh[] {
            //    Addressables.LoadAssetAsync<Mesh>(RoR2_Base_ClayBruiser.mdlClayBruiser_fbx_ClayBruiserMesh_).WaitForCompletion(),
            //    Addressables.LoadAssetAsync<Mesh>(RoR2_Base_ClayBruiser.mdlClayBruiser_fbx_ClayBruiserCannonMesh_).WaitForCompletion(),
            //    Addressables.LoadAssetAsync<Mesh>(RoR2_Base_ClayBruiser.mdlClayBruiser_fbx_ClayBruiserChestArmorMesh_).WaitForCompletion(),
            //    Addressables.LoadAssetAsync<Mesh>(RoR2_Base_ClayBruiser.mdlClayBruiser_fbx_ClayBruiserHeadMesh_).WaitForCompletion(),
            //};

            //SkinDefParams.MeshReplacement[] ogMeshReplacements = ogSkinDefParams.meshReplacements;
            //skinDef.skinDefParams.meshReplacements = new SkinDefParams.MeshReplacement[ogMeshReplacements.Length];
            //ogMeshReplacements.CopyTo(skinDef.skinDefParams.meshReplacements, 0);
            //for (int i = 0; i < ogMeshReplacements.Length; i++) {
            //    skinDef.skinDefParams.meshReplacements[i].renderer = skinDef.skinDefParams.rendererInfos[i].renderer;
            //    skinDef.skinDefParams.meshReplacements[i].mesh = ogMeshes.Where(mesh => mesh.name == skinDef.skinDefParams.meshReplacements[i].renderer.gameObject.name).FirstOrDefault();
            //    skinDef.skinDefParams.meshReplacements[i].meshAddress = default;
            //}

            //skinDef.skinDefParams.gameObjectActivations = Array.Empty<SkinDefParams.GameObjectActivation>();
            //skinDef.skinDefParams.meshReplacements = Array.Empty<SkinDefParams.MeshReplacement>();
            //skinDef.skinDefParams.projectileGhostReplacements = Array.Empty<SkinDefParams.ProjectileGhostReplacement>();
            //skinDef.skinDefParams.minionSkinReplacements = Array.Empty<SkinDefParams.MinionSkinReplacement>();
            skinDef.nameToken = "Templar_DEFAULT_SKIN";
            skinDef.name = "Templar_DEFAULT_SKIN";
            #endregion

            //var skinDefs = new List<SkinDef>()
            //{
            //    skinDef
            //};

            //skinController.skins = skinDefs.ToArray();
        }
    }
}