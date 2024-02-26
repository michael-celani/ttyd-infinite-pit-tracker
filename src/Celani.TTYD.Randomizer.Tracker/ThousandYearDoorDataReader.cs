using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System.Text;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class ThousandYearDoorDataReader(GamecubeGame game)
    {
        /// <summary>
        /// The Gamecube Game.
        /// </summary>
        private GamecubeGame Game { get; set; } = game ?? throw new ArgumentNullException(nameof(game));

        /// <summary>
        /// The address of the file name.
        /// </summary>
        private const long FileNameAddress = 0x803dbdd4;

        /// <summary>
        /// The address of the Frame Retrace.
        /// </summary>
        private const long FrameRetraceAddress = 0x803dac48;

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
        public PlayerStats Pouch { get; private set; } = new PlayerStats();

        /// <summary>
        /// The information about the mod.
        /// </summary>
        public InfinitePitStats ModInfo { get; private set; } = new InfinitePitStats();

        // Small buffers used for reading small data.
        private readonly byte[] _smallbuf = new byte[8];
        private readonly byte[] _tickbuff = new byte[8];

        /// <summary>
        /// Updates the memory.
        /// </summary>
        public void Update()
        {
            Pouch.Read(Game);
            ModInfo.Read(Game);

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
            FileName = Encoding.ASCII.GetString(_smallbuf).Replace('?', '♡').Trim('\0');
        }
    }
}
