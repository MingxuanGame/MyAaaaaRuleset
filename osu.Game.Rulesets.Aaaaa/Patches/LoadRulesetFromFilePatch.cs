using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using osu.Framework;
using osu.Framework.Logging;

namespace osu.Game.Rulesets.Aaaaa.Patches
{
    [HarmonyPatch(typeof(RulesetStore), "loadRulesetFromFile")]
    public class LoadRulesetFromFilePatch
    {
        private static bool hasRescanned;

        private static FieldInfo loadedAssembliesField;

        static bool Prefix(RulesetStore __instance, string file, ref Assembly __result)
        {
            string filename = Path.GetFileNameWithoutExtension(file);

            Logger.Log($"[Aaaaa] loadRulesetFromFile: {filename}");

            // Skip test files (same as original)
            if (filename.Contains("Tests"))
            {
                Logger.Log($"[Aaaaa] Skip test file: {filename}");
                __result = null;
                return false;
            }

            // Load assembly and register directly — bypass checkValidForRelease entirely
            try
            {
                Assembly assembly = Assembly.LoadFrom(file);

                loadedAssembliesField ??= typeof(RulesetStore).GetField("LoadedAssemblies",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                var loadedAssemblies = loadedAssembliesField?.GetValue(__instance) as Dictionary<Assembly, Type>;

                if (loadedAssemblies == null)
                {
                    Logger.Log($"[Aaaaa] CRITICAL: cannot access LoadedAssemblies");
                    __result = null;
                    return false;
                }

                // Dedup: same as original logic
                if (loadedAssemblies.ContainsKey(assembly) ||
                    loadedAssemblies.Any(kvp => kvp.Key.FullName == assembly.FullName))
                {
                    Logger.Log($"[Aaaaa] Already loaded: {filename}");
                    __result = assembly;
                    return false;
                }

                Type rulesetType = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(Ruleset)));

                loadedAssemblies[assembly] = rulesetType;
                Logger.Log($"[Aaaaa] Registered: {filename} ({rulesetType.FullName})");
                __result = assembly;
            }
            catch (Exception ex)
            {
                Logger.Log($"[Aaaaa] FAILED {filename}: {ex.Message}");
                __result = null;
            }

            // One-shot rescan: catch all DLLs that appeared before our patch was active
            if (!hasRescanned)
            {
                hasRescanned = true;
                Logger.Log($"[Aaaaa] === Starting one-shot rescan ===");
                RescanMissedRulesets(__instance);
                Logger.Log($"[Aaaaa] === Rescan complete ===");
            }

            return false;
        }

        private static void RescanMissedRulesets(RulesetStore instance)
        {
            try
            {
                string startupDir = RuntimeInfo.StartupDirectory;

                if (string.IsNullOrEmpty(startupDir))
                {
                    Logger.Log($"[Aaaaa] Rescan skipped: no StartupDirectory");
                    return;
                }

                loadedAssembliesField ??= typeof(RulesetStore).GetField("LoadedAssemblies",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                var loadedAssemblies = loadedAssembliesField?.GetValue(instance) as Dictionary<Assembly, Type>;

                if (loadedAssemblies == null)
                {
                    Logger.Log($"[Aaaaa] Rescan: cannot access LoadedAssemblies");
                    return;
                }

                var existingFiles = new HashSet<string>(
                    loadedAssemblies.Values
                        .Select(t => Path.GetFileNameWithoutExtension(t.Assembly.Location)));

                var missedFiles = Directory.GetFiles(startupDir, "osu.Game.Rulesets.*.dll", SearchOption.AllDirectories)
                    .Where(f => !Path.GetFileName(f).Contains("Tests"))
                    .Where(f => !existingFiles.Contains(Path.GetFileNameWithoutExtension(f)));

                int count = 0;

                foreach (var file in missedFiles)
                {
                    try
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        var assembly = Assembly.LoadFrom(file);

                        if (loadedAssemblies.ContainsKey(assembly) ||
                            loadedAssemblies.Any(kvp => kvp.Key.FullName == assembly.FullName))
                            continue;

                        Type type = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(Ruleset)));
                        loadedAssemblies[assembly] = type;
                        Logger.Log($"[Aaaaa] Rescan loaded: {name} ({type.FullName})");
                        count++;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"[Aaaaa] Rescan FAILED {Path.GetFileNameWithoutExtension(file)}: {ex.Message}");
                    }
                }

                Logger.Log($"[Aaaaa] Rescan: loaded {count} new ruleset(s)");
            }
            catch (Exception ex)
            {
                Logger.Log($"[Aaaaa] Rescan error: {ex.Message}");
            }
        }
    }
}
