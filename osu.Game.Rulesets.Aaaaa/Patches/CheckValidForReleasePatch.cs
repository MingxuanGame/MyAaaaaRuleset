using HarmonyLib;

namespace osu.Game.Rulesets.Aaaaa.Patches;

[HarmonyPatch(typeof(RulesetStore), "checkValidForRelease")]
public class CheckValidForReleasePatch
{
    static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}
