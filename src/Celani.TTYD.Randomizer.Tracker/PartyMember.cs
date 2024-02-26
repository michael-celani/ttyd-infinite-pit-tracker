using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Celani.TTYD.Randomizer.Tracker
{
    /// <summary>
    /// Represents a party member.
    /// </summary>
    /// <param name="Data">Backing data for this party member.</param>
    public class PartyMember(Memory<byte> Data)
    {
        public ref PouchPartyMember GetPouchPartyMember() => ref MemoryMarshal.AsRef<PouchPartyMember>(Data.Span);

        [JsonPropertyName("flags")]
        public ushort Flags
        {
            get
            {
                ref var pouch = ref GetPouchPartyMember();
                return pouch.flags;
            }
        }

        [JsonPropertyName("tech_level")]
        public short TechLevel
        {
            get
            {
                ref var pouch = ref GetPouchPartyMember();
                return pouch.tech_level;
            }
        }
    }
}
