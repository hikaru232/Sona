using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sona.Models
{
    public class Song
    {
        public string FilePath { get; set; } = "";
        public int Volume { get; set; } = 50;
        public string Name { get; set; } = "";
        public string Hotkey { get; set; } = "";
    }
}
