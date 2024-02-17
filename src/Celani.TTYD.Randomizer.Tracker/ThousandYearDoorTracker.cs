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

        private readonly byte[] PouchDataBuffer = new byte[Marshal.SizeOf<PouchData>()];
        private readonly byte[] ModDataBuffer = new byte[Marshal.SizeOf<ModData>()];
        private readonly byte[] TickBuffer = new byte[8];
        private readonly byte[] FilenameBuffer = new byte[8];

        public ref PouchData GetPouchData() => ref MemoryMarshal.AsRef<PouchData>(PouchDataBuffer);

        public ref ModData GetModData() => ref MemoryMarshal.AsRef<ModData>(ModDataBuffer);

        /// <summary>
        /// Updates the memory.
        /// </summary>
        public void Update()
        {
            // Read the pouch memory.
            Game.Read(PouchAddress, PouchDataBuffer);
            PouchDataBuffer.AsSpan().Reverse();

            // Read the ModData.
            Game.Read(ModStateAddress, ModDataBuffer);
            ModDataBuffer.AsSpan().Reverse();

            // Read the filename.
            Game.Read(FileNameAddress, FilenameBuffer);
            FileName = Encoding.ASCII.GetString(FilenameBuffer).Replace('?', '♡');

            // Read the tick.
            Game.Read(FrameRetraceAddress, TickBuffer);
            TickBuffer.AsSpan().Reverse();

            Tick = BitConverter.ToUInt64(TickBuffer);
        }
    }
}
