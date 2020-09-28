using System;
using System.Collections.Generic;

namespace Conlanger
{
    public class ConsonantList
    {

        public ConsonantList(IEnumerable<string> filtered)
        {
            All = new List<string>();
            Leaders = new List<string>();
            Medians = new List<string>();
            Trailers = new List<string>();
            Process(filtered);

        }

        private void Process(IEnumerable<string> filtered)
        {

            foreach (var con in filtered)
            {
                if (con[0] != '<') All.Add(con);
                else if (con[1] == 'S') Leaders.Add(con);
                else if (con[1] == 'M') Medians.Add(con);
                else if (con[1] == 'E') Trailers.Add(con);
            }
        }

        public List<string> All { get; set; }
        public List<string> Leaders { get; set; }
        public List<string> Medians { get; set; }
        public List<string> Trailers { get; set; }


    }
}