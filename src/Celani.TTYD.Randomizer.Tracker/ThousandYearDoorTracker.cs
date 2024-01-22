using Celani.TTYD.Randomizer.Tracker.Dolphin;
using System.Runtime.InteropServices;
using System.Text;

namespace Celani.TTYD.Randomizer.Tracker
{
    public class ThousandYearDoorTracker
    {
        /// <summary>
        /// The Gamecube Game.
        /// </summary>
        private GamecubeGame Game { get; set; }

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
        /// The data in the pouch.
        /// </summary>
        public PouchData PouchData { get; set; }

        /// <summary>
        /// The mod data.
        /// </summary>
        public ModData ModData { get; set; }

        /// <summary>
        /// The file name.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// The current tick.
        /// </summary>
        public ulong Tick { get; set; }

        /// <summary>
        /// Constructs a new Tracker.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ThousandYearDoorTracker(GamecubeGame game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        /// <summary>
        /// Updates the memory.
        /// </summary>
        public void Update()
        {
            // Read the pouch memory.
            byte[] pouchMemory = Game.Read(PouchAddress, Marshal.SizeOf<PouchData>());
            Array.Reverse(pouchMemory);
            PouchData = PinMemory<PouchData>(pouchMemory);

            Array.Reverse(PouchData.party_data);
            Array.Reverse(PouchData.key_items);
            Array.Reverse(PouchData.items);
            Array.Reverse(PouchData.stored_items);
            Array.Reverse(PouchData.badges);
            Array.Reverse(PouchData.equipped_badges);

            // Read the filename.
            FileName = Game.ReadString(FileNameAddress, 8).Replace('?', '♡');

            // Read the ModData.
            byte[] modMemory = Game.Read(ModStateAddress, Marshal.SizeOf<ModData>());
            Array.Reverse(modMemory);
            ModData = PinMemory<ModData>(modMemory);

            // Read the tick.
            byte[] tickMemory = Game.Read(FrameRetraceAddress, 8);
            Array.Reverse(tickMemory);
            Tick = BitConverter.ToUInt64(tickMemory);
        }

        private static T PinMemory<T>(byte[] memory)
        {
            GCHandle handle = GCHandle.Alloc(memory, GCHandleType.Pinned);
            T data = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            handle.Free();

            return data;

        }
    }
}
