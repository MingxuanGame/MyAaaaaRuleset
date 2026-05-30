using System;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using osu.Framework;

namespace osu.Game.Rulesets.Aaaaa.Patches
{
    [HarmonyPatch(typeof(RulesetStore), "loadFromDisk")]
    public class LoadFromDiskPatch
    {
        static void Postfix(RulesetStore __instance)
        {
            var startupDir = RuntimeInfo.StartupDirectory;
            if (string.IsNullOrEmpty(startupDir))
                return;

            try
            {
                var files = Directory.GetFiles(startupDir, "osu.Game.Rulesets.*.dll", SearchOption.AllDirectories)
                    .Where(f => !Path.GetFileName(f).Contains("Tests"));

                var addRuleset = typeof(RulesetStore).GetMethod("addRuleset",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (addRuleset == null)
                    return;

                foreach (var file in files)
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(file);
                        addRuleset.Invoke(__instance, new object[] { assembly });
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }
    }
}
