using osu.Game.Beatmaps;
using osu.Game.Rulesets.Aaaaa.Objects;

namespace osu.Game.Rulesets.Aaaaa.Beatmaps
{
    public class AaaaaBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
        : BeatmapConverter<AaaaaHitObject>(beatmap, ruleset)
    {
        public override bool CanConvert() => false;
    }
}
