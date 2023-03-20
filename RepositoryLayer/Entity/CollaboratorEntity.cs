using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public long CollabId { get; set; }
        public string CollabEmail { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public long NotesId { get; set; }

        [ForeignKey("NotesId")]
        public NotesEntity Notes { get; set; }


    }
}
