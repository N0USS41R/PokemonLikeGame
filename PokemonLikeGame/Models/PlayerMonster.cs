using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExerciceMonster.Models
{
    public class PlayerMonster
    {
        [Key, Column(Order = 0)]
        public int PlayerID { get; set; }

        [Key, Column(Order = 1)]
        public int MonsterID { get; set; }

        [ForeignKey("PlayerID")]
        public virtual Player Player { get; set; }

        [ForeignKey("MonsterID")]
        public virtual Monster Monster { get; set; }
    }
}
