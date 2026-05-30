using System;
using System.Collections.Generic;

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Overlays;
using osu.Game.Rulesets.Aaaaa.Beatmaps;
using osu.Game.Rulesets.Aaaaa.UI;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Aaaaa
{
    public partial class AaaaaRuleset : Ruleset
    {
        private const string short_name = "aaaaaruleset";

        public AaaaaRuleset()
        {
        }

        public override string Description => "My learning project of osu lazer ruleset";

        public override string ShortName => short_name;

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) =>
            new DrawableAaaaaRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new AaaaaBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new AaaaaDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            return type switch
            {
                _ => Array.Empty<Mod>()
            };
        }

        public override Drawable CreateIcon() => new AaaaaIcon();

        public partial class AaaaaIcon : CompositeDrawable
        {
            public AaaaaIcon()
            {
                AutoSizeAxes = Axes.Both;
                InternalChildren =
                [
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = FontAwesome.Regular.Circle,
                    },
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.5f),
                        Icon = FontAwesome.Solid.Unlock,
                    }
                ];
            }
        }
    }
}
