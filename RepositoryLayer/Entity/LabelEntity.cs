using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }
        public string LabelName { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public long NotesId { get; set; }

        [ForeignKey("NotesId")]
        public NotesEntity Notes { get; set; }
    }
}
