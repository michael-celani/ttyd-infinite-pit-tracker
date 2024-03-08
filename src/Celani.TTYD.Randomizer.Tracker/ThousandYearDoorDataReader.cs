using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System.Runtime.InteropServices;
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
        /// The address of the pouch.
        /// </summary>
        private const long PouchAddress = 0x80b07b60;

        /// <summary>
        /// The address of the file name.
        /// </summary>
        private const long FileNameAddress = 0x803dbdd4;

        /// <summary>
        /// The address of the Frame Retrace.
        /// </summary>
        private const long FrameRetraceAddress = 0x803dac48;

        /// <summary>
        /// The address of the mod state.
        /// </summary>
        private const long ModStateAddress = 0x80b56aa0;

        /// <summary>
        /// The address of the RTA final time.
        /// </summary>
        private const long FinalTimeAddress = 0x80b56538;

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
        public byte[] Pouch { get; private set; } = new byte[Marshal.SizeOf<PouchData>()];

        /// <summary>
        /// The information about the mod.
        /// </summary>
        public byte[] ModInfo { get; private set; } = new byte[Marshal.SizeOf<ModData>()];

        /// <summary>
        /// The information about the final time.
        /// </summary>
        public byte[] TimeData { get; private set; } = new byte[Marshal.SizeOf<FinalTimeData>()];

        // Small buffers used for reading small data.
        private readonly byte[] _smallbuf = new byte[8];
        private readonly byte[] _tickbuff = new byte[8];

        /// <summary>
        /// Updates the memory.
        /// </summary>
        public bool Update()
        {
            if (!Game.Running)
            {
                return false;
            }

            // Read the ModData.
            if (Game.Read(PouchAddress, Pouch))
            {
                Pouch.AsSpan().Reverse();
            }
            else
            {
                return false;
            }

            // Read the ModData.
            if (Game.Read(ModStateAddress, ModInfo))
            {
                ModInfo.AsSpan().Reverse();
            }
            else
            {
                return false;
            }

            // Read the final time.
            if (Game.Read(FinalTimeAddress, TimeData))
            {
                TimeData.AsSpan().Reverse();
            }
            else
            {
                return false;
            }

            // Read the tick.
            if (Game.Read(FrameRetraceAddress, _tickbuff))
            {
                _tickbuff.AsSpan().Reverse();
                Tick = BitConverter.ToUInt64(_tickbuff);
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the filename.
        /// </summary>
        public void UpdateFilename()
        {
            // Read the filename.
            if (Game.Read(FileNameAddress, _smallbuf))
            {
                FileName = Encoding.ASCII.GetString(_smallbuf).Replace('?', '♡').Trim('\0');
            }
        }
    }
}
