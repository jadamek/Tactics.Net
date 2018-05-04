using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Tactics.Net.Maps;

namespace Tactics.Net.Movement
{
    //========================================================================================================================
    // ** Mobile Object Interface
    //========================================================================================================================
    // An Isometric object with a 3-Dimensional position, and a 2.5-d tilemap environment
    //========================================================================================================================
    public interface IMobile
    {
        Vector3f Position { get; set; }
        Map Environment { get; set; }
    }
}
