using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public class AttackerSimulationWrapper : ObservableObject
    {
        #region Constructor
        public AttackerSimulationWrapper (Pokemon attacker = null, BattleResult result = null)
        {
            if (attacker == null)
            {
                Pokemon newPokemon = new Pokemon();
                newPokemon.IVSets.Add(new IVSet());
                this.Attacker = newPokemon;
            }
            else
            {
                this.Attacker = attacker;
            }
            if (result == null)
            {
                this.BattleResult = new BattleResult();
            }
            else
            {
                this.BattleResult = result;
            }
        }
        #endregion
        #region Public Properties
        private Pokemon _Attacker;
        public Pokemon Attacker
        {
            get
            {
                return this._Attacker;
            }
            set
            {
                this.Set(ref this._Attacker, value);
            }
        }
        private BattleResult _BattleResult;
        public BattleResult BattleResult
        {
            get
            {
                return this._BattleResult;
            }
            set
            {
                this.Set(ref this._BattleResult, value);
            }
        }
        #endregion
    }
}
