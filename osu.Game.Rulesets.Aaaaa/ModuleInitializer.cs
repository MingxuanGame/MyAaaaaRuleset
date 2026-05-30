using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace osu.Game.Rulesets.Aaaaa
{
    internal static class AaaaaModuleInitializer
    {
#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
        internal static void Initialize()
        {
            var harmony = new Harmony("aaaaaruleset");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
