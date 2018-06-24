using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Base.Enums;
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
        public BattleSimulationViewModel(NavigationService navigationService, SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this.navigationService = navigationService;
            this._messageViewer = messageViewer;
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

        #region Private Fields
        private MessageViewerBase _messageViewer;
        private enum BattleState { Idle, FastAttackInit, FastAttack, ChargeAttackInit, ChargeAttack, Dodging }
        #endregion
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
            const int timeInterval = 100;
            int attackerDamageWindowStartTime;
            int defenderDamageWindowStartTime;
            bool defenderUseChargeMove = false;
            int attackerLivesNeeded = 0;
            Random rand = new Random();
            double averageTime = 0.0;
            double averageLives = 0.0;
            double averageDPS = 0.0;
            double averageTDO = 0.0;
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
                int attackerStartAttackTime = 0;
                int defenderStartAttackTime = 0;
                int dodgeWindowStartTime = 0;
                int dodgeEndTime = 0;
                if (IsRaidBoss)
                {
                    int index;
                    if (!Constants.RaidBosses.TryGetValue(this.Defender.Species.Species, out index))
                    {
                        if (!errorShown)
                            this._messageViewer.DisplayMessage($"{Defender.Species.Species} is not a defined raid boss!", "Undefined Raid Boss", MessageViewerButton.Ok).Wait();
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
                int defenderStartHP = defenderHP;
                int time = 0; //Time in msec
                double timeRemaining;
                BattleState attackerState = BattleState.Idle;
                BattleState defenderState = BattleState.Idle;
                int attackerPower = 0, defenderPower = 0;
                double attackerBonus = 1.0, defenderBonus = 1.0;
                while (attackerHP > 0 && time < 1000000)
                {
                    int attackerDamage = 0, defenderDamage = 0;
                    double currentDPS = 0.0, currentTDO = 0.0;
                    if (attackerState == BattleState.Idle)
                    {
                        attackerPower = 0;
                        attackerBonus = 1.0;
                        if (time >= attackerNextAttackTime)
                        {
                            if (this.DodgeChargeAttacks && defenderState == BattleState.ChargeAttack)
                            {
                                if (time >= dodgeWindowStartTime)
                                {
                                    dodgeEndTime = time + Constants.dodgeDurationMs;
                                    attackerState = BattleState.Dodging;
                                }
                            }
                            else if (attackerEnergy >= Attacker.ChargeMove.ChargeMove.Energy)
                            {
                                attackerState = BattleState.ChargeAttackInit;
                                attackerStartAttackTime = time + Attacker.ChargeMove.ChargeMove.DamageWindowStartTime;
                                attackerNextAttackTime = time + Attacker.ChargeMove.ChargeMove.Time;
                            }
                            else if (attackerEnergy < Attacker.ChargeMove.ChargeMove.Energy)
                            {
                                attackerState = BattleState.FastAttackInit;
                                attackerStartAttackTime = time + Attacker.FastMove.FastMove.DamageWindowStartTime;
                                attackerNextAttackTime = time + Attacker.FastMove.FastMove.Time;
                            }
                        }
                    }
                    if (attackerState == BattleState.FastAttackInit)
                    {
                        if (time >= attackerStartAttackTime)
                        {
                            attackerPower = Attacker.FastMove.FastMove.Power;
                            if (Attacker.Species.Type1 == Attacker.FastMove.FastMove.Type || Attacker.Species.Type2 == Attacker.FastMove.FastMove.Type)
                                attackerBonus *= Constants.StabBonus;
                            attackerBonus *= Constants.CalculateTypeBonus(Attacker.FastMove.FastMove.Type, Defender.Species.Type1, Defender.Species.Type2);
                            attackerEnergy += Attacker.FastMove.FastMove.Energy;
                            attackerState = BattleState.FastAttack;
                        }
                    }
                    if (attackerState == BattleState.FastAttack)
                    {
                        if (time >= attackerStartAttackTime && time < attackerStartAttackTime + Attacker.FastMove.FastMove.DamageWindowDuration)
                        {
                            attackerDamage = (int)(timeInterval / (double)Attacker.FastMove.FastMove.DamageWindowDuration * Constants.CalculateDamage(attackerPower, Attacker.GetAttack(), Defender.GetDefense(), attackerBonus));
                        }
                        if (time >= attackerNextAttackTime)
                        {
                            attackerState = BattleState.Idle;
                        }
                    }
                    if (attackerState == BattleState.ChargeAttackInit)
                    {
                        if (time >= attackerStartAttackTime)
                        {
                            attackerPower = Attacker.ChargeMove.ChargeMove.Power;
                            attackerNextAttackTime = time + Attacker.ChargeMove.ChargeMove.Time;
                            attackerEnergy -= Attacker.ChargeMove.ChargeMove.Energy;
                            if (Attacker.Species.Type1 == Attacker.ChargeMove.ChargeMove.Type || Attacker.Species.Type2 == Attacker.ChargeMove.ChargeMove.Type)
                                attackerBonus *= Constants.StabBonus;
                            attackerBonus *= Constants.CalculateTypeBonus(Attacker.ChargeMove.ChargeMove.Type, Defender.Species.Type1, Defender.Species.Type2);
                            attackerState = BattleState.ChargeAttack;
                        }
                    }
                    if (attackerState == BattleState.ChargeAttack)
                    {
                        if (time >= attackerStartAttackTime && time < attackerStartAttackTime + Attacker.ChargeMove.ChargeMove.DamageWindowDuration)
                        {
                            attackerDamage = (int)(timeInterval / (double)Attacker.ChargeMove.ChargeMove.DamageWindowDuration * Constants.CalculateDamage(attackerPower, Attacker.GetAttack(), Defender.GetDefense(), attackerBonus));
                        }
                        if (time >= attackerNextAttackTime)
                        {
                            attackerState = BattleState.Idle;
                        }
                    }
                    if (attackerState == BattleState.Dodging)
                    {
                        if (time >= dodgeEndTime)
                            attackerState = BattleState.Idle;
                    }

                    //Defender logic
                    if (defenderState == BattleState.Idle)
                    {
                        defenderPower = 0;
                        defenderBonus = 1.0;
                        if (time >= defenderNextAttackTime)
                        {
                            if (defenderEnergy >= Defender.ChargeMove.ChargeMove.Energy && (rand.Next(0, 10) >= 5))
                            {
                                defenderState = BattleState.ChargeAttackInit;
                                defenderStartAttackTime = time + Defender.ChargeMove.ChargeMove.DamageWindowStartTime;
                                defenderNextAttackTime = time + Defender.ChargeMove.ChargeMove.Time;
                            }
                            else if (defenderEnergy < Defender.ChargeMove.ChargeMove.Energy)
                            {
                                defenderState = BattleState.FastAttackInit;
                                defenderStartAttackTime = time + Defender.FastMove.FastMove.DamageWindowStartTime;
                                defenderNextAttackTime = time + Defender.FastMove.FastMove.Time + Constants.DefenderFastMoveDelay;
                            }
                        }
                    }
                    if (defenderState == BattleState.FastAttackInit)
                    {
                        if (time >= defenderStartAttackTime)
                        {
                            defenderPower = Defender.FastMove.FastMove.Power;
                            if (Defender.Species.Type1 == Defender.FastMove.FastMove.Type || Defender.Species.Type2 == Defender.FastMove.FastMove.Type)
                                defenderBonus *= Constants.StabBonus;
                            defenderBonus *= Constants.CalculateTypeBonus(Defender.FastMove.FastMove.Type, Attacker.Species.Type1, Attacker.Species.Type2);
                            defenderEnergy += Defender.FastMove.FastMove.Energy;
                            defenderState = BattleState.FastAttack;
                        }
                    }
                    if (defenderState == BattleState.FastAttack)
                    {
                        if (time >= defenderStartAttackTime && time < defenderStartAttackTime + Defender.FastMove.FastMove.DamageWindowDuration)
                        {
                            defenderDamage = (int)(timeInterval / (double)Attacker.FastMove.FastMove.DamageWindowDuration * Constants.CalculateDamage(defenderPower, Defender.GetAttack(), Attacker.GetDefense(), defenderBonus));
                        }
                        if (time >= defenderNextAttackTime)
                        {
                            defenderState = BattleState.Idle;
                        }
                    }
                    if (defenderState == BattleState.ChargeAttackInit)
                    {
                        if (time >= defenderStartAttackTime)
                        {
                            defenderPower = Defender.ChargeMove.ChargeMove.Power;
                            defenderNextAttackTime = time + Defender.ChargeMove.ChargeMove.Time;
                            defenderEnergy -= Defender.ChargeMove.ChargeMove.Energy;
                            if (Defender.Species.Type1 == Defender.ChargeMove.ChargeMove.Type || Defender.Species.Type2 == Defender.ChargeMove.ChargeMove.Type)
                                defenderBonus *= Constants.StabBonus;
                            defenderBonus *= Constants.CalculateTypeBonus(Defender.ChargeMove.ChargeMove.Type, Attacker.Species.Type1, Attacker.Species.Type2);
                            defenderState = BattleState.ChargeAttack;
                        }
                    }
                    if (defenderState == BattleState.ChargeAttack)
                    {
                        if (time >= defenderStartAttackTime && time < defenderStartAttackTime + Defender.ChargeMove.ChargeMove.DamageWindowDuration)
                        {
                            defenderDamage = (int)(timeInterval / (double)Attacker.ChargeMove.ChargeMove.DamageWindowDuration * Constants.CalculateDamage(defenderPower, Defender.GetAttack(), Attacker.GetDefense(), defenderBonus));
                        }
                        if (time >= defenderNextAttackTime)
                        {
                            defenderState = BattleState.Idle;
                        }
                    }
                    if (defenderState == BattleState.Dodging)
                    {
                        defenderState = BattleState.Idle;
                    }

                    //Damage and energy exchange
                    if (attackerState == BattleState.Dodging)
                        defenderDamage = (int)(Constants.dodgeDamageFactor * (double)defenderDamage);
                    if (attackerDamage > 0)
                    {
                        defenderHP -= attackerDamage;
                        defenderEnergy += (int)Math.Ceiling(attackerDamage / 2.0);
                    }
                    if (defenderDamage > 0)
                    {
                        attackerHP -= defenderDamage;
                        attackerEnergy += (int)Math.Ceiling(defenderDamage / 2.0);
                    }

                    //Limit energy to 100
                    defenderEnergy = defenderEnergy > 100 ? 100 : defenderEnergy;
                    attackerEnergy = attackerEnergy > 100 ? 100 : attackerEnergy;

                    //if (attackerHP <= 0)
                    //{
                    //    attackerLivesNeeded++;
                    //    attackerHP = Attacker.ActualHP;
                    //    attackerEnergy = 0;
                    //    attackerNextAttackTime = time + Constants.PokemonSwitchDelayMs;
                    //}
                    if (defenderHP <= 0)
                    {
                        break;
                    }
                    time += timeInterval;
                    if (this.IsRaidBoss)
                        timeRemaining = Constants.RaidTimer - time / 1000.0;
                    else
                        timeRemaining = Constants.GymTimer - time / 1000.0;
                }
                averageDPS += (double)(defenderStartHP - defenderHP) / ((double)(time - timeInterval) / 1000.0);
                averageTDO += (double)(defenderStartHP - defenderHP) / (double)defenderStartHP;
                averageTime += (time - timeInterval) / 1000.0;
                averageLives += attackerLivesNeeded;
            }
            this.BattleResult = new BattleResult()
            {
                BattleDuration = averageTime / (double)Constants.NumSimulations,
                NumberOfDeaths = averageLives / (double)Constants.NumSimulations,
                DPS = averageDPS / (double)Constants.NumSimulations,
                TDO = averageTDO / (double)Constants.NumSimulations
            };
        }
        #endregion
    }
}
