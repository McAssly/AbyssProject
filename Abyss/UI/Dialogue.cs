﻿
namespace Abyss.UI
{
    internal class Dialogue
    {
        public Dialogue[] Next; // the multiple possible new dialogues to move on from
        public Text master; // master text or rather the message being displayed
        public Text[] options; // the options given to the player to choose from in order to continue the dialogue tree

        public Dialogue(Text master, Text[] options = null, Dialogue[] next = null)
        {
            this.master = master;
            this.Next = next;
            this.options = options;
        }
    }
}
