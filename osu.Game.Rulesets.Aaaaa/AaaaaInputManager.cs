using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Aaaaa
{
    public partial class AaaaaInputManager(RulesetInfo ruleset)
        : RulesetInputManager<AaaaaAction>(ruleset, 0, SimultaneousBindingMode.Unique);

    public enum AaaaaAction;
}
