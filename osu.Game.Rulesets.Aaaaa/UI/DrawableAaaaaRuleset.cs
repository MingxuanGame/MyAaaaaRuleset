using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Aaaaa.Objects;
using osu.Game.Rulesets.Aaaaa.Objects.Drawables;
using osu.Game.Rulesets.Aaaaa.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Aaaaa.UI
{
    [Cached]
    public partial class DrawableAaaaaRuleset(
        AaaaaRuleset ruleset,
        IBeatmap beatmap,
        IReadOnlyList<Mod> mods = null)
        : DrawableRuleset<AaaaaHitObject>(ruleset, beatmap, mods)
    {
        protected override Playfield CreatePlayfield() => new AaaaaPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) =>
            new AaaaaFramedReplayInputHandler(replay);

        public override DrawableHitObject<AaaaaHitObject>
            CreateDrawableRepresentation(AaaaaHitObject h) => new DrawableAaaaaHitObject(h);

        protected override PassThroughInputManager CreateInputManager() =>
            new AaaaaInputManager(Ruleset?.RulesetInfo);
    }
}
