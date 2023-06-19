using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abyss.UI
{
    internal class UiManager
    {
        // Declare every single UI menu in the game
        public static Ui Dialogue { get; set; }
        public static Ui Debug { get; set; }
        public static Ui Invenetory { get; set; }
        public static Ui Shop { get; set; }
        public static Ui Main { get; set; }
        public static Ui Options { get; set; }


        private Ui current;

        public UiManager(Ui current) 
        {
            this.current = current;
        }

        public Ui GetCurrent() { return current; }
        public void SetCurrent(Ui current) { this.current = current;}
    }
}
