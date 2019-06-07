using System.Collections.Generic;

namespace CostAndPackBreakdown
{
    /// <summary>
    /// Use to store a list of packs and total size together.
    /// </summary>
    class Packs
    {
        public List<Pack> PackList { get; set; }
        public int TotalSize { get; set; }

        public Packs() { }
    }
}
