using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Models.Helpers
{
    public static class RandomHelper
    {

        public static int GetRouletteResult()
        {
            Random random = new Random();
            return random.Next(0, 36);
        }
    }
}
