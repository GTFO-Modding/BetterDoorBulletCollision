using AssetShards;
using GameData;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterDoorBulletCollision.Inject
{
    [HarmonyPatch(typeof(ComplexResourceSetDataBlock), nameof(ComplexResourceSetDataBlock.PopulateFullResourceArrays))]
    internal static class Inject_ResourceSet_Populate
    {
        public static int Layer_ProjectileBlocker = 0;

        private static void Prefix(ComplexResourceSetDataBlock __instance)
        {
            Layer_ProjectileBlocker = LayerMask.NameToLayer("ProjectileBlocker");

            if (__instance.SmallWeakGates != null)
            {
                ApplyBetterDoorCollision(__instance.SmallWeakGates.ToArray());
            }

            if (__instance.MediumWeakGates != null)
            {
                ApplyBetterDoorCollision(__instance.MediumWeakGates.ToArray());
            }

            if (__instance.LargeWeakGates != null)
            {
                ApplyBetterDoorCollision(__instance.LargeWeakGates.ToArray());
            }
        }

        private static void ApplyBetterDoorCollision(ResourceData[] datas)
        {
            foreach (var data in datas)
            {
                var door = AssetShardManager.GetLoadedAsset<GameObject>(data.Prefab, false);
                if (door == null)
                {
                    continue;
                }

                if (door.GetComponent<DoorBladeCollisionHandler>() != null)
                {
                    continue;
                }

                var spawner = door.GetComponent<DebrisSpawner>();
                if (spawner == null)
                {
                    continue;
                }
                var root = spawner.DebrisObjectsRoot;
                var doorBaseRenderer = root.GetComponentInChildren<SkinnedMeshRenderer>();
                if (doorBaseRenderer == null)
                {
                    continue;
                }

                doorBaseRenderer.gameObject.layer = Layer_ProjectileBlocker;
                var collider = doorBaseRenderer.gameObject.AddComponent<MeshCollider>();
                var mat = doorBaseRenderer.gameObject.AddComponent<ColliderMaterial>();
                mat.m_FX_GroupOverride = FX_EffectSystem.FX_GroupName.Impact_Metal;

                var handler = door.AddComponent<DoorBladeCollisionHandler>();
                handler.Spawner.Set(spawner);
                handler.MainCollider.Set(collider);
                handler.MainRenderer.Set(doorBaseRenderer);
            }
        }
    }
}
