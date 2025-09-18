using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GenericDBAccessor.Models
{
    [Table("book")]
    internal class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [Column("authorId")]
        public int AuthorId { get; set; }  
        [Column("year")]
        public int PublishingYear { get; set; }
        [Column("pages")]
        public int NumberOfPages {  get; set; }
    }
}
