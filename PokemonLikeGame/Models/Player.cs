using ExerciceMonster.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExerciceMonster.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("Login")]
        public int LoginID { get; set; }

        public virtual Login Login { get; set; }

        public virtual ICollection<PlayerMonster> PlayerMonsters { get; set; }
    }
}
