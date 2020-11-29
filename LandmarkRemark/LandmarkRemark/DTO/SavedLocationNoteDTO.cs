using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace LandmarkRemark.DTO
{
    public class SavedLocationNoteDTO
    {
        public int UserID { get; set; }
        public decimal CoordinateX { get; set; }
        public decimal CoordinateY { get; set; }
        public string MessageNote { get; set; }
    }
}
