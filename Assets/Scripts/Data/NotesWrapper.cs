using System;
using System.Collections.Generic;

namespace KJakub.Octave.Data
{
    [Serializable]
    public class NotesWrapper
    {
        public List<NoteData> Notes { get; set; }
    }
}