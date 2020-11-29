using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandmarkRemark.DTO
{
    public class SavedLocationNoteDTO
    {
        public string UserID { get; set; }
        public string LocationCoordinates { get; set; }

        public string MessageNote { get; set; }
    }
}
