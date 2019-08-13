using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RC.Database
{
    [Table("agent_responses")]
    public class AgentResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 0)]
        public int Id { get; set; }

        [Column("name", Order = 1, TypeName = "varchar(127)")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        [Column("created_at", Order = 2, TypeName = "timestamptz")]
        public DateTime CreatedAt { get; set; }

        [Column("a", Order = 3, TypeName = "int")]
        public int A { get; set; }

        [Column("b", Order = 4, TypeName = "int")]
        public int B { get; set; }

        [Column("c", Order = 5, TypeName = "int")]
        public int C { get; set; }
    }
}
