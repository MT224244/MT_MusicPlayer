using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT_MusicPlayer.Models
{
    public class Music
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public TimeSpan TotalTime { get; set; }
    }
}
