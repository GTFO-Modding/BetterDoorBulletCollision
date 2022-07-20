using BepInEx.IL2CPP.Utils;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterDoorBulletCollision
{
    internal sealed class DoorBladeCollisionHandler : MonoBehaviour
    {
        public Il2CppReferenceField<DebrisSpawner> Spawner;
        public Il2CppReferenceField<SkinnedMeshRenderer> MainRenderer;
        public Il2CppReferenceField<MeshCollider> MainCollider;

        [HideFromIl2Cpp]
        public void RunCollision()
        {
            var spawner = Spawner.Value;
            var collidersToRemove = new List<Collider>();
            foreach (var des in spawner.m_destructionObjects)
            {
                if (des.m_debris)
                {
                    collidersToRemove.Add(des.m_DestructionObject.GetComponent<Collider>());
                }
            }

            this.StartCoroutine(UpdateDoorMeshCollider(collidersToRemove.ToArray()));
        }

        [HideFromIl2Cpp]
        private IEnumerator UpdateDoorMeshCollider(Collider[] collidersToRemove)
        {
            yield return new WaitForSeconds(0.35f);

            var length = collidersToRemove.Length;
            for (int i = 0; i < length; i++)
            {
                Destroy(collidersToRemove[i]);
            }

            yield return null;

            var renderer = MainRenderer.Value;
            while (true)
            {
                if (renderer.isVisible)
                {
                    var colliderMesh = new Mesh();
                    MainRenderer.Value.BakeMesh(colliderMesh);
                    MainCollider.Value.sharedMesh = null;
                    MainCollider.Value.sharedMesh = colliderMesh;
                    break;
                }
                yield return null;
            }
        }
    }
}
