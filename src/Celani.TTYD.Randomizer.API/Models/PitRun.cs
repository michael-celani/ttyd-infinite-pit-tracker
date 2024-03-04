using Celani.TTYD.Randomizer.API.Converters;
using Celani.TTYD.Randomizer.Tracker;
using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.API.Models
{
    /// <summary>
    /// A wrapper around ThousandYearDoorDataReader that tracks the state of the Pit of 100 Trials.
    /// </summary>
    [JsonConverter(typeof(PitRunConverter))]
    public class PitRun(ThousandYearDoorDataReader data)
    {
        public bool InGame => RunStart.HasValue;

        public bool IsFinished { get; set; }

        public int CurrentFloor { get; set; } = -1;

        public DateTime? RunStart { get; set; }

        public DateTime? CurrentFloorStart { get; set; }

        public DateTime Now { get; set; }

        public PitLog PitLog { get; set; } = new();

        /// <summary>
        /// Event that is raised when a run starts.
        /// </summary>
        public event EventHandler OnPitStart;

        /// <summary>
        /// Event that is raised when a run is reset.
        /// </summary>
        public event EventHandler OnPitReset;

        /// <summary>
        /// Event that is raised when a run is finished.
        /// </summary>
        public event EventHandler OnPitFinish;

        /// <summary>
        /// The data reader for the game.
        /// </summary>
        public ThousandYearDoorDataReader Data { get; set; } = data; 
        
        /// <summary>
        /// Calculates the elapsed time since the start of the run.
        /// </summary>
        /// <returns>The elapsed time as a TimeSpan.</returns>
        public TimeSpan GetRunElapsed() => !RunStart.HasValue ? TimeSpan.Zero : Now - RunStart.Value;

        /// <summary>
        /// Calculates the elapsed time since the start of the current floor.
        /// </summary>
        /// <returns>The elapsed time as a TimeSpan.</returns>
        public TimeSpan GetFloorElapsed() => !CurrentFloorStart.HasValue ? TimeSpan.Zero : Now - CurrentFloorStart.Value;

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

        /// <summary>
        /// Updates the state of the Pit of 100 Trials.
        /// </summary>
        public bool Update()
        {
            if (!Data.Update())
            {
                return false;
            }

            Now = GamecubeGame.DateTimeFromGCNTick(
                Data.ModInfo.PitFinished ? 
                Data.ModInfo.PitEndTime : 
                Data.Tick
            );

            // The pit is not running in the game.
            if (Data.ModInfo.PitStartTime == 0)
            {
                // The pit was reset: we think that the pit
                // is running, but the game does not.
                if (InGame)
                {
                    Reset();
                    OnRaisePitReset();
                }

                return true;
            }

            // The floor has updated.
            if (Data.ModInfo.Floor != CurrentFloor)
            {
                // A file was loaded: the game is running the
                // pit, but we think it's stopped.
                if (!InGame)
                {
                    Start();
                    OnRaisePitStart();
                }
                else
                {
                    // We actually changed floors, snapshot the previous one:
                    Snapshot();
                }

                CurrentFloor = Data.ModInfo.Floor;
                CurrentFloorStart = Now;
            }

            // The run has finished by reading the sign.
            if (!IsFinished && Data.ModInfo.PitFinished)
            {
                Finish();
                Snapshot();
                OnRaisePitFinish();
            }

            return true;
        }

        /// <summary>
        /// Snapshots the current floor.
        /// </summary>
        private void Snapshot()
        {
            var snapshot = new FloorSnapshot
            {
                Floor = CurrentFloor,
                FloorDuration = GetFloorElapsed(),
                FloorEndPouch = new PlayerStatsSlim(Data.Pouch),
                FloorEndStats = new InfinitePitStatsSlim(Data.ModInfo)
            };

            PitLog.FloorSnapshots.Add(snapshot);
        }

        private void Start()
        {
            RunStart = GamecubeGame.DateTimeFromGCNTick(Data.ModInfo.PitStartTime);
            Data.UpdateFilename();
            PitLog.Seed = Data.FileName;
        }

        public void Reset()
        {
            IsFinished = false;
            RunStart = null;
            CurrentFloor = -1;
            CurrentFloorStart = null;
            PitLog.FloorSnapshots.Clear();
            Data.UpdateFilename();
        }

        public void Finish()
        {
            IsFinished = true;
        }
    }
}
