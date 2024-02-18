using Celani.TTYD.Randomizer.Tracker.Extensions;
using Celani.TTYD.Randomizer.Tracker.Windows;
using ProcessMemoryUtilities.Managed;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Celani.TTYD.Randomizer.Tracker.Dolphin
{
    public partial class GamecubeGame
    {
        /// <summary>
        /// The Dolphin process.
        /// </summary>
        private Process Game { get; set; }

        /// <summary>
        /// The base address for Gamecube games.
        /// </summary>
        public static readonly long BaseAddressGC = 0x80000000;

        /// <summary>
        /// The basic information about the mapped MEM1 memory.
        /// </summary>
        private MemoryBasicInformation Memory { get; set; }

        /// <summary>
        /// The GameCube epoch, January 1 2000.
        /// </summary>
        private static DateTime Epoch { get; set; } = new DateTime(2000, 1, 1);

        /// <summary>
        /// The number of GameCube ticks in a millisecond.
        /// </summary>
        private static ulong TicksPerMillisecond { get; set; } = 40500;

        /// <summary>
        /// A regular expression that validates game codes.
        /// </summary>
        /// <returns>The game code regular expression.</returns>
        [GeneratedRegex("^[A-Z0-9]{6}$")]
        private static partial Regex GameCodeRegex();

        internal GamecubeGame(Process dolphin, MemoryBasicInformation memory)
        {
            Game = dolphin ?? throw new ArgumentNullException(nameof(dolphin));
            Memory = memory;
        }

        /// <summary>
        /// Creates a new Gamecube game from a Dolphin process.
        /// </summary>
        /// <param name="dolphin">The Dolphin process.</param>
        /// <returns>The GameCubeGame representing the played game.</returns>
        /// <exception cref="InvalidOperationException">No MEM1 was found in the process.</exception>
        public static GamecubeGame Create(Process dolphin)
        {
            // Find MEM1:
            MemoryBasicInformation? mem1 = dolphin.EnumerateVirtualMemory().Where(memory => IsGameCubeMEM1(dolphin, memory)).FirstOrDefault();

            if (!mem1.HasValue)
                throw new InvalidOperationException("MEM1 not found in process.");

            return new GamecubeGame(dolphin, mem1.Value);
        }

        public void Read(long gcAddress, byte[] buffer)
        {
            nint procAddr = ConvertToProcessAddress(gcAddress);

            // Read from the game:
            NativeWrapper.ReadProcessMemoryArray(Game.Handle, procAddr, buffer);
        }

        /// <summary>
        /// Checks if a given string is a valid game code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>true if the game code is valid, false otherwise.</returns>
        public static bool ValidateGameCode(ReadOnlySpan<char> code) => GameCodeRegex().IsMatch(code);

        /// <summary>
        /// Converts a GCN tick to a DateTime.
        /// </summary>
        /// <param name="tick">The GCN Tick.</param>
        /// <returns>The DateTime representing the tick.</returns>
        public static DateTime DateTimeFromGCNTick(ulong tick) => Epoch + TimeSpan.FromMilliseconds(tick / TicksPerMillisecond);

        /// <summary>
        /// Converts the GameCube address to the process address.
        /// </summary>
        /// <param name="gcAddress">The GameCube address.</param>
        /// <returns>The process address location.</returns>
        private nint ConvertToProcessAddress(long gcAddress) => (nint) (gcAddress - BaseAddressGC + Memory.BaseAddress);

        /// <summary>
        /// Checks if a memory page is likely to be the GameCube MEM1.
        /// </summary>
        /// <param name="process">The Dolphin process.</param>
        /// <param name="information">The information.</param>
        /// <returns>True if the memory page is likely GameCube MEM1.</returns>
        private static bool IsGameCubeMEM1(Process process, MemoryBasicInformation information)
        {
            if (information.RegionSize != 0x2000000)
                return false;

            if (information.Type != TypeEnum.MEM_MAPPED)
                return false;

            if (!information.Protect.HasFlag(AllocationProtectEnum.PAGE_READWRITE))
                return false;

            // If this is the mapped memory, its first six bytes should be the game code:
            byte[] buffer = new byte[6];

            // Read the memory:
            NativeWrapper.ReadProcessMemoryArray(process.Handle, information.BaseAddress, buffer);

            string codeString = Encoding.ASCII.GetString(buffer);

            return ValidateGameCode(codeString);
        }
    }
}
