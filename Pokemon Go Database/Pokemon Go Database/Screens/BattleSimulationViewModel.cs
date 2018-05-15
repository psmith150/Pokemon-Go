using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Model;
using Pokemon_Go_Database.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pokemon_Go_Database.Screens
{
    public class BattleSimulationViewModel : ScreenViewModel
    {
        private readonly NavigationService navigationService;
        #region Commands
        public ICommand SimulateBattleCommand { get; private set; }
        #endregion

        #region Constructor
        public BattleSimulationViewModel(NavigationService navigationService, SessionService session) : base(session)
        {
            this.navigationService = navigationService;
            this.Attacker = new Pokemon();
            this.Defender = new Pokemon();
            this.BattleResult = new BattleResult();

            this.SimulateBattleCommand = new RelayCommand(async () => await Task.Run(() => this.SimulateBattleAsync()));
        }
        #endregion

        public override void Initialize()
        {
        }

        public override void Deinitialize()
        {
        }
        #region Public Properties
        private Pokemon _Attacker;
        public Pokemon Attacker
        {
            get
            {
                return _Attacker;
            }

            set
            {
                Set(ref this._Attacker, value);
            }
        }
        private Pokemon _Defender;
        public Pokemon Defender
        {
            get
            {
                return _Defender;
            }

            set
            {
                Set(ref this._Defender, value);
            }
        }
        private Pokemon _SelectedAttackerPokemon;
        public Pokemon SelectedAttackerPokemon
        {
            get
            {
                return this._SelectedAttackerPokemon;
            }
            set
            {
                Set(ref this._SelectedAttackerPokemon, value);
                if (this.SelectedAttackerPokemon != null)
                    this.Attacker = this.SelectedAttackerPokemon.Copy();
            }
        }
        private Pokemon _SelectedDefenderPokemon;
        public Pokemon SelectedDefenderPokemon
        {
            get
            {
                return this._SelectedDefenderPokemon;
            }
            set
            {
                Set(ref this._SelectedDefenderPokemon, value);
                if (this.SelectedDefenderPokemon != null)
                    this.Defender = this.SelectedDefenderPokemon.Copy();
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
                Set(ref this._BattleResult, value);
            }
        }

        public bool IsRaidBoss { get; set; }
        public bool DodgeChargeAttacks { get; set; }
        #endregion

        #region Public Methods
        public void SimulateBattleAsync()
        {
            if (this.Attacker == null || this.Defender == null)
                return;

            int attackerEnergyLostTime;
            int defenderEnergyLostTime;
            bool defenderUseChargeMove = false;
            int attackerLivesNeeded = 0;
            Random rand = new Random();
            double averageTime = 0.0;
            double averageLives = 0.0;
            bool errorShown = false;
            for (int i = 0; i < Constants.NumSimulations; i++)
            {
                attackerLivesNeeded = 0;
                int attackerHP = this.Attacker.ActualHP;
                int defenderHP = this.Defender.ActualHP * 2;
                int attackerEnergy = 0;
                int defenderEnergy = 0;
                int attackerNextAttackTime = 700;
                int defenderNextAttackTime = 1600;
                if (IsRaidBoss)
                {
                    int index;
                    if (!Constants.RaidBosses.TryGetValue(this.Defender.Species.Species, out index))
                    {
                        if (!errorShown)
                            MessageBox.Show($"{Defender.Species.Species} is not a defined raid boss!", "Undefined Raid Boss", MessageBoxButton.OK);
                        errorShown = true;
                    }
                    else
                    {
                        defenderHP = Constants.RaidBossHP[index - 1];
                        this.Defender.AttackIVExpression = Constants.MaxIV.ToString();
                        this.Defender.DefenseIVExpression = Constants.MaxIV.ToString();
                        this.Defender.StaminaIVExpression = Constants.MaxIV.ToString();
                        this.Defender.LevelExpression = Constants.RaidBossLevels[index - 1].ToString();
                    }
                }
                int time = 0; //Time in msec
                double timeRemaining;
                while (defenderHP > 0)
                {
                    int attackerDamage = 0, defenderDamage = 0;
                    if (time >= attackerNextAttackTime)
                    {
                        int power = 0;
                        double bonus = 1.0;
                        //Use charge move
                        if (attackerEnergy >= Attacker.ChargeMove.ChargeMove.Energy)
                        {
                            power = Attacker.ChargeMove.ChargeMove.Power;
                            attackerNextAttackTime = time + Attacker.ChargeMove.ChargeMove.Time;
                            attackerEnergy -= Attacker.ChargeMove.ChargeMove.Energy;
                            if (Attacker.Species.Type1 == Attacker.ChargeMove.ChargeMove.Type || Attacker.Species.Type2 == Attacker.ChargeMove.ChargeMove.Type)
                                bonus *= Constants.StabBonus;
                            bonus *= Constants.CalculateTypeBonus(Attacker.ChargeMove.ChargeMove.Type, Defender.Species.Type1, Defender.Species.Type2);
                        }
                        //Use fast move
                        else
                        {
                            power = Attacker.FastMove.FastMove.Power;
                            attackerNextAttackTime = time + Attacker.FastMove.FastMove.Time;
                            attackerEnergy += Attacker.FastMove.FastMove.Energy;
                            if (Attacker.Species.Type1 == Attacker.FastMove.FastMove.Type || Attacker.Species.Type2 == Attacker.FastMove.FastMove.Type)
                                bonus *= Constants.StabBonus;
                            bonus *= Constants.CalculateTypeBonus(Attacker.FastMove.FastMove.Type, Defender.Species.Type1, Defender.Species.Type2);
                        }

                        attackerDamage = Constants.CalculateDamage(power, Attacker.GetAttack(), Defender.GetDefense(), bonus);
                        //Debug.WriteLine($"Hariyama used")
                    }
                    if (time >= defenderNextAttackTime)
                    {
                        int power = 0;
                        double bonus = 1.0;
                        bool usingChargeMove = false;
                        //Check for charge move
                        if (defenderEnergy >= Defender.ChargeMove.ChargeMove.Energy)
                        {
                            //Use charge move
                            if (defenderUseChargeMove)
                            {
                                power = Defender.ChargeMove.ChargeMove.Power;
                                defenderNextAttackTime = time + Defender.ChargeMove.ChargeMove.Time;
                                defenderEnergy -= Defender.ChargeMove.ChargeMove.Energy;
                                if (Defender.Species.Type1 == Defender.ChargeMove.ChargeMove.Type || Defender.Species.Type2 == Defender.ChargeMove.ChargeMove.Type)
                                    bonus *= Constants.StabBonus;
                                bonus *= Constants.CalculateTypeBonus(Defender.ChargeMove.ChargeMove.Type, Attacker.Species.Type1, Attacker.Species.Type2);
                                defenderUseChargeMove = false;
                                usingChargeMove = true;
                            }
                            //Use fast move
                            else
                            {
                                defenderUseChargeMove = rand.Next(0, 10) >= 5;
                                power = Defender.FastMove.FastMove.Power;
                                defenderNextAttackTime = time + Defender.FastMove.FastMove.Time + Constants.DefenderFastMoveDelay;
                                defenderEnergy += Defender.FastMove.FastMove.Energy;
                                if (Defender.Species.Type1 == Defender.FastMove.FastMove.Type || Defender.Species.Type2 == Defender.FastMove.FastMove.Type)
                                    bonus *= Constants.StabBonus;
                                bonus *= Constants.CalculateTypeBonus(Defender.FastMove.FastMove.Type, Attacker.Species.Type1, Attacker.Species.Type2);
                            }
                        }
                        //Use fast move
                        else
                        {
                            power = Defender.FastMove.FastMove.Power;
                            defenderNextAttackTime = time + Defender.FastMove.FastMove.Time + Constants.DefenderFastMoveDelay;
                            defenderEnergy += Defender.FastMove.FastMove.Energy;
                            if (Defender.Species.Type1 == Defender.FastMove.FastMove.Type || Defender.Species.Type2 == Defender.FastMove.FastMove.Type)
                                bonus *= Constants.StabBonus;
                            bonus *= Constants.CalculateTypeBonus(Defender.FastMove.FastMove.Type, Attacker.Species.Type1, Attacker.Species.Type2);
                        }
                        defenderDamage = Constants.CalculateDamage(power, Defender.GetAttack(), Attacker.GetDefense(), bonus);
                        if (usingChargeMove && this.DodgeChargeAttacks)
                        {
                            defenderDamage = (int)Math.Max(1, Math.Floor(0.25 * defenderDamage));
                            attackerNextAttackTime = Math.Max(attackerNextAttackTime, time + Constants.dodgeDurationMs);
                        }
                    }
                    defenderHP -= attackerDamage;
                    defenderEnergy += (int)Math.Ceiling(attackerDamage / 2.0);
                    attackerHP -= defenderDamage;
                    attackerEnergy += (int)Math.Ceiling(defenderDamage / 2.0);

                    //Limit energy to 100
                    defenderEnergy = defenderEnergy > 100 ? 100 : defenderEnergy;
                    attackerEnergy = attackerEnergy > 100 ? 100 : attackerEnergy;

                    if (attackerHP <= 0)
                    {
                        attackerLivesNeeded++;
                        attackerHP = Attacker.ActualHP;
                        attackerEnergy = 0;
                        attackerNextAttackTime = time + Constants.PokemonSwitchDelayMs;
                    }
                    time += 100;
                    timeRemaining = Constants.GymTimer - time / 1000.0;
                }
                averageTime += (time - 100) / 1000.0;
                averageLives += attackerLivesNeeded;
            }
            this.BattleResult = new BattleResult()
            {
                BattleDuration = averageTime / (double)Constants.NumSimulations,
                NumberOfDeaths = averageLives / (double)Constants.NumSimulations
            };
        }
        #endregion
    }
}
