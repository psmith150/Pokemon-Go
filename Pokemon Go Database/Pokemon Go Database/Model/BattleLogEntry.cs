using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class BattleLogEntry
    {
        public BattleLogEntry(int time, int attackerHP, int attackerEnergy, int defenderHP, int defenderEnergy, string attackerAction = "None", string defenderAction = "None")
        {
            this.Time = time;
            this.AttackerHP = attackerHP;
            this.AttackerEnergy = attackerEnergy;
            this.DefenderHP = defenderHP;
            this.DefenderEnergy = defenderEnergy;
            this.AttackerAction = attackerAction;
            this.DefenderAction = defenderAction;
        }

        public int Time { get; set; }
        public int AttackerHP { get; set; }
        public int AttackerEnergy { get; set; }
        public int DefenderHP { get; set; }
        public int DefenderEnergy { get; set; }
        public string AttackerAction { get; set; }
        public string DefenderAction { get; set; }
    }
}
