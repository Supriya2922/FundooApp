using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer
{
    public class AddCollabModel
    {
        public long NoteId { get; set; }

        public string email { get; set; }
    }
}
