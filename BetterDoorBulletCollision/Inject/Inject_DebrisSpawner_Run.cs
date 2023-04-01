using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterDoorBulletCollision.Inject
{
    [HarmonyPatch(typeof(DebrisSpawner), nameof(DebrisSpawner.RunSequence))]
    [HarmonyPatch(new Type[] { })]
    internal static class Inject_DebrisSpawner_Run
    {
        private static void Prefix(DebrisSpawner __instance)
        {
            var handler = __instance.GetComponent<DoorBladeCollisionHandler>();
            if (handler != null)
            {
                handler.RunCollision();
            }
        }
    }
}
