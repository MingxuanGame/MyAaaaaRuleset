using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Replays;
using osuTK;

namespace osu.Game.Rulesets.Aaaaa.Replays
{
    public class AaaaaReplayFrame : ReplayFrame
    {
        public List<AaaaaAction> Actions = [];
        public Vector2 Position;

        public AaaaaReplayFrame(AaaaaAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }

        public override bool IsEquivalentTo(ReplayFrame other)
            => other is AaaaaReplayFrame freeformFrame && Time == freeformFrame.Time &&
               Position == freeformFrame.Position && Actions.SequenceEqual(freeformFrame.Actions);
    }
}
