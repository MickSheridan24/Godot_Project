
using System;
using System.Collections.Generic;
using System.Linq;

namespace Conlanger
{


    public class ProtoAncient
    {

        public string[] vowels { get; set; }
        public ConsonantList consonants { get; set; }
        public string consonantString { get; set; }
        public bool Declined { get; }
        public bool Inflected { get; }
        public bool Compounds { get; }
        public bool AttachPreps { get; }
        private Random random { get; }
        public ProtoAncient()
        {

            random = new Random();

            vowels = new string[]{
                "a", "ae", "au", "e", "ei", "eo", "eu", "i", "io", "ia", "iu", "ie", "o", "ou", "oi", "oa", "oe", "u", "uu", "uo", "ui", "ua", "ue"
            };

            consonantString = "<S>q[r,s,u],r,<S>rr,t[r,l,s],<M>tt,p[h,r,l,s],s[l,p,t,k,c,m,d]," +
                         "d[r,l],f[r,l],<M>ff,g[l,r,h,n],h[r],<M>hh,k[r,l,s,h],<M>kk,l,<M>ll," +
                         "c[s,r,l,n],<M>cc,v[r,l,h],b[r,l],<M>bb,n<M>nn,m[r,h],<M>mm";

            Declined = true;
            Inflected = true;
            Compounds = false;
            AttachPreps = random.Next(2) == 0;

            NarrowVowels();
            consonants = AssembleCL();
        }

        private void NarrowVowels()
        {
            var omits = 10;
            vowels = vowels.Where(v => random.Next(5) == 0 && omits-- > 0).ToArray();
        }

        private ConsonantList AssembleCL()
        {
            var omits = 30;

            var conCodes = new List<string>();

            var i = 0;
            while (i < consonantString.Length)
            {
                var newCon = GetNextConsonant(i, out i);

                conCodes.Add(newCon);
            }

            var final = conCodes.Where(c => !c.Contains('[')).ToList();

            var toUnpack = conCodes.Where(c => c.Contains('['));


            foreach (var code in toUnpack)
            {
                var newCodes = Unpack(code);
                final.AddRange(newCodes);
            }

            var filtered = final.Where(c => random.Next(5) == 0 && omits-- > 0);

            return new ConsonantList(filtered);
        }

        private IEnumerable<string> Unpack(string code)
        {
            var tag = "";
            var i = 0;
            var segments = code.Split('[');
            if (code[0] == '<')
            {
                tag = code.Substring(0, 3);
                i = 3;
            }

            var character = segments[0].Substring(i);

            var ret = new List<string>();

            var blends = segments[1].Trim(']').Split(',');

            foreach (var blend in blends)
            {
                ret.Add(tag + character + blend);
            }
            return ret;
        }

        private string GetNextConsonant(int i, out int o)
        {
            var ret = "";
            ret += consonantString[i];
            var inSquare = false;
            while (i < consonantString.Length)
            {
                i++;
                var currChar = consonantString[i];
                if (!inSquare && currChar == ',')
                {
                    break;
                }
                else if (inSquare && currChar == ']')
                {
                    inSquare = false;
                }
                else if (!inSquare && currChar == '[')
                {
                    inSquare = true;
                }
                ret += consonantString[i];
            }
            o = i;
            return ret;
        }

    }
}


