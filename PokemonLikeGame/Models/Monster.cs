using ExerciceMonster.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExerciceMonster.Models
{
    public class Monster
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Health { get; set; }

        public string ImageURL { get; set; }

        public virtual ICollection<MonsterSpell> MonsterSpells { get; set; }
    }
}
