using System;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Gives identity and ownership to any class implementing this interface.
    /// </summary>
    public interface IIdentifiable
    {
        Identification MyID { get; }
    }

    /// <summary>
    /// Stores relevant identification information such as player ownership and faction assignment.
    /// </summary>
    [Serializable()]
    public class Identification
    {
        public int playerID = 0;
        public int factionID = 0;

        public Identification(int playerID, int factionID)
        {
            this.playerID = playerID;
            this.factionID = factionID;
        }
    }
}
