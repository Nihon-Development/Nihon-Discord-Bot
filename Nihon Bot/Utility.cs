using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NihonBot
{
    public static class Utility
    {
        public static ulong OwnerID = 875397959834017822; /* Owner Discord ID */
        public static ulong Server = 881976771707301888; /* Server ID */

        public static Emote GetEmote(string EmoteName, string EmoteID) /* Pass EmoteName And ID Gather By \:EmoteName: In Discord Client */
        { 
            return Emote.Parse($"<:{EmoteName}:{EmoteID}>"); /* Parse And Return Emote "GetAnimatedEmote" Works Same Just Returns Animated Emote */
        }

        public static Emote GetAnimatedEmote(string EmoteName, string EmoteID) 
        { 
            return Emote.Parse($"<a:{EmoteName}:{EmoteID}>"); 
        }
    }
}
