using osu.Framework.Allocation;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Aaaaa.UI
{
    [Cached]
    public partial class AaaaaPlayfield : Playfield
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AddRangeInternal([
                HitObjectContainer
            ]);
        }
    }
}
