using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System.Runtime.InteropServices;
using System.Text;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class ThousandYearDoorTracker(GamecubeGame game)
    {
        /// <summary>
        /// The Gamecube Game.
        /// </summary>
        private GamecubeGame Game { get; set; } = game ?? throw new ArgumentNullException(nameof(game));

        /// <summary>
        /// The address of the file name.
        /// </summary>
        private static readonly long FileNameAddress = 0x803dbdd4;

        /// <summary>
        /// The address of the pouch in TTYD memory.
        /// </summary>
        private static readonly long PouchAddress = 0x80b07b60;

        /// <summary>
        /// The address of the mod state in TTYD memory.
        /// </summary>
        private static readonly long ModStateAddress = 0x80b56aa0;

        /// <summary>
        /// The address of the Frame Retrace.
        /// </summary>
        private static readonly long FrameRetraceAddress = 0x803dac48;

        /// <summary>
        /// The file name.
        /// </summary>
        public string FileName { get; private set; } = string.Empty;

        /// <summary>
        /// The current tick.
        /// </summary>
        public ulong Tick { get; private set; }

        /// <summary>
        /// The pouch, which represents party data.
        /// </summary>
        public Pouch Pouch { get; private set; } = new Pouch();

        /// <summary>
        /// The information about the mod.
        /// </summary>
        public InfinitePit ModInfo { get; private set; } = new InfinitePit();

        // Small buffers used for reading small data.
        private readonly byte[] _smallbuf = new byte[8];
        private readonly byte[] _tickbuff = new byte[8];

        /// <summary>
        /// Updates the memory.
        /// </summary>
        public void Update()
        {
            // Read the pouch memory.
            Game.Read(PouchAddress, Pouch.Data);
            Pouch.Data.AsSpan().Reverse();

            // Read the ModData.
            Game.Read(ModStateAddress, ModInfo.Data);
            ModInfo.Data.AsSpan().Reverse();

            // Read the tick.
            Game.Read(FrameRetraceAddress, _tickbuff);
            _tickbuff.AsSpan().Reverse();
            Tick = BitConverter.ToUInt64(_tickbuff);
        }

        /// <summary>
        /// Updates the filename.
        /// </summary>
        public void UpdateFilename()
        {
            // Read the filename.
            Game.Read(FileNameAddress, _smallbuf);
            FileName = Encoding.ASCII.GetString(_smallbuf).Replace('?', '♡');
        }
    }
}
