using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExerciceMonster.Models
{
    public class MonsterSpell
    {
        [Key, Column(Order = 0)]
        public int MonsterID { get; set; }

        [Key, Column(Order = 1)]
        public int SpellID { get; set; }

        [ForeignKey("MonsterID")]
        public virtual Monster Monster { get; set; }

        [ForeignKey("SpellID")]
        public virtual Spell Spell { get; set; }
    }
}
