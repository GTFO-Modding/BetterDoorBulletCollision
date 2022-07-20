using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System.Linq;

namespace BetterDoorBulletCollision
{
    [BepInPlugin("BetterDoorBulletCollision", VersionInfo.RootNamespace, VersionInfo.Version)]
    internal class EntryPoint : BasePlugin
    {
        private Harmony _Harmony;

        public override void Load()
        {
            _Harmony = new Harmony($"{VersionInfo.RootNamespace}.Harmony");
            _Harmony.PatchAll();

            ClassInjector.RegisterTypeInIl2Cpp<DoorBladeCollisionHandler>();
        }
    }
}