using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RC.Database
{
    [Table("agents")]
    public class Agent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 0)]
        public int Id { get; set; }

        [Column("host", Order = 1, TypeName = "varchar(2047)")]
        public string Host { get; set; }

        [Column("attempts", Order = 3, TypeName = "int")]
        public int Attempts { get; set; }

        public Agent()
        {
            Attempts = 0;
        }
    }
}
