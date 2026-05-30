using System.Collections.Generic;
using System.Linq;
using osu.Framework.Input.StateChanges;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Aaaaa.Replays
{
    public class AaaaaFramedReplayInputHandler(Replay replay)
        : FramedReplayInputHandler<AaaaaReplayFrame>(replay)
    {
        protected override bool IsImportant(AaaaaReplayFrame frame) => frame.Actions.Any();

        protected override void CollectReplayInputs(List<IInput> inputs)
        {
        }
    }
}
