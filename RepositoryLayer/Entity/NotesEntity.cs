using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RepositoryLayer.Entity
{
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotesId { get; set; }
        public string Title { get; set; }   
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string BackgroundColor { get; set; }
        public string Image { get; set; }
       
        public bool isArchieved { get; set; }
        public bool isPinned { get; set; }
        public bool trash { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set;}
        public long UserId { get; set; }

       [ForeignKey("UserId")]
        public UserEntity User { get; set; }

    }
}
