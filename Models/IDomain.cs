using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.Models
{
    public interface IDomain
    {
        int Id { get; set; }

        string CreateKey();
    }
}
