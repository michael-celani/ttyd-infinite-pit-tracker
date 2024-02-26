using Celani.TTYD.Randomizer.API.Converters;
using Celani.TTYD.Randomizer.Tracker;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.API.Models
{
    /// <summary>
    /// Information about a currently active Pit run.
    /// </summary>
    [JsonConverter(typeof(PitRunConverter))]
    public class PitRun
    {
        public bool InGame { get; set; }

        public bool IsFinished { get; set; }

        public int CurrentFloor { get; set; } = -1;

        public DateTime? CurrentFloorStart { get; set; }

        public List<FloorSnapshot> FloorSnapshots { get; set; } = [];

        public event EventHandler OnPitStart;

        public event EventHandler OnPitReset;

        public event EventHandler OnPitFinish;

        /// <summary>
        /// Calculates how much time has elapsed on the current floor from the given <seealso cref="TimeSpan"/>.
        /// </summary>
        /// <param name="now">The current time.</param>
        /// <returns>The amount of time elapsed on the current floor.</returns>
        public TimeSpan GetFloorElapsed(DateTime now) => !CurrentFloorStart.HasValue ? TimeSpan.Zero : now - CurrentFloorStart.Value;

        private void OnRaisePitStart()
        {
            EventHandler raiseEvent = OnPitStart;

            if (raiseEvent is not null)
            {
                raiseEvent(this, EventArgs.Empty);
            }
        }

        private void OnRaisePitReset()
        {
            EventHandler raiseEvent = OnPitReset;

            if (raiseEvent is not null)
            {
                raiseEvent(this, EventArgs.Empty);
            }
        }

        private void OnRaisePitFinish()
        {
            EventHandler raiseEvent = OnPitFinish;

            if (raiseEvent is not null)
            {
                raiseEvent(this, EventArgs.Empty);
            }
        }

        public void Update(PlayerStats playerInfo, InfinitePitStats modInfo, DateTime now)
        {
            // The pit is not running.
            if (modInfo.PitStartTime == 0)
            {
                // The pit was started, but just ended.
                if (InGame)
                {
                    Reset();
                    OnRaisePitReset();
                }

                return;
            }

            // The floor has updated.
            if (modInfo.Floor != CurrentFloor)
            {
                if (!InGame)
                {
                    // The game has started.
                    InGame = true;
                    OnRaisePitStart();
                }
                else if (!IsFinished)
                {
                    Snapshot(playerInfo, modInfo, now);
                }

                CurrentFloor = modInfo.Floor;
                CurrentFloorStart = now;
            }

            // The run has finished.
            if (!IsFinished && modInfo.PitFinished)
            {
                IsFinished = modInfo.PitFinished;
                Snapshot(playerInfo, modInfo, now);
                OnRaisePitFinish();
            }
        }

        /// <summary>
        /// Snapshots the current floor.
        /// </summary>
        /// <param name="playerInfo">The pouch.</param>
        /// <param name="modInfo">The mod info.</param>
        /// <param name="now">The current time.</param>
        private void Snapshot(PlayerStats playerInfo, InfinitePitStats modInfo, DateTime now)
        {
            var snapshot = new FloorSnapshot
            {
                Floor = CurrentFloor,
                FloorDuration = GetFloorElapsed(now),
                FloorEndPouch = new PlayerStats(playerInfo),
                FloorEndStats = new InfinitePitStats(modInfo)
            };

            FloorSnapshots.Add(snapshot);
        }

        public void Reset()
        {
            InGame = false;
            IsFinished = false;
            CurrentFloor = -1;
            CurrentFloorStart = null;
            FloorSnapshots.Clear();
        }
    }
}
