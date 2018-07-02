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
            this.BattleLog = new List<BattleLogEntry>();

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
        private enum BattleState { Idle, FastAttackInit, FastAttackWindow, ChargeAttackInit, ChargeAttackWindow, Dodging }
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
                if (this.Attacker.IVSets.Count <= 0)
                    this.Attacker.IVSets.Add(new IVSet());
                else if (this.Attacker.IVSets.Count > 1)
                {
                    while (this.Attacker.IVSets.Count > 1)
                        this.Attacker.IVSets.RemoveAt(1);
                }

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
                if (this.Defender.IVSets.Count <= 0)
                    this.Defender.IVSets.Add(new IVSet());
                else if (this.Defender.IVSets.Count > 1)
                {
                    while (this.Defender.IVSets.Count > 1)
                        this.Defender.IVSets.RemoveAt(1);
                }
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

        private double _NextBreakpoint;
        public double NextBreakpoint
        {
            get
            {
                return this._NextBreakpoint;
            }
            set
            {
                this.Set(ref this._NextBreakpoint, value);
            }
        }

        private List<BattleLogEntry> _BattleLog;
        public List<BattleLogEntry> BattleLog
        {
            get
            {
                return this._BattleLog;
            }
            set
            {
                this.Set(ref this._BattleLog, value);
            }
        }
        private bool _IsRaidBoss;
        public bool IsRaidBoss
        {
            get
            {
                return this._IsRaidBoss;
            }
            set
            {
                this.Set(ref this._IsRaidBoss, value);
            }
        }

        public Array DefenderTypes
        {
            get
            {
                return Enum.GetValues(typeof(DefenderType));
            }
        }
        private DefenderType _DefenderType;
        public DefenderType DefenderType
        {
            get
            {
                return this._DefenderType;
            }
            set
            {
                this.Set(ref this._DefenderType, value);
                if (this.DefenderType != DefenderType.GymDefender)
                {
                    this.Defender.IVSets[0].AttackIV = Constants.MaxIV;
                    this.Defender.IVSets[0].DefenseIV = Constants.MaxIV;
                    this.Defender.IVSets[0].StaminaIV = Constants.MaxIV;
                    int index;
                    if (!Constants.RaidBosses.TryGetValue(this.Defender.Species.Species, out index) || index != (int)this.DefenderType)
                    {
                        this._messageViewer.DisplayMessage($"{Defender.Species.Species} is not a normal raid boss at this tier!", "Undefined Raid Boss", MessageViewerButton.Ok, MessageViewerIcon.Warning);
                    }
                    double level = 1.0;
                    switch (this.DefenderType)
                    {
                        case DefenderType.Tier1:
                        case DefenderType.Tier2:
                        case DefenderType.Tier3:
                        case DefenderType.Tier4:
                            level = 30.0;
                            break;
                        case DefenderType.Tier5:
                            level = 40.0;
                            break;
                        default:
                            level = 1.0;
                            break;

                    }
                    this.Defender.IVSets[0].Level = level;
                    this.IsRaidBoss = true;
                }
                else
                    this.IsRaidBoss = false;
            }
        }
        public bool DodgeChargeAttacks { get; set; }
        #endregion

        #region Public Methods
        public void SimulateBattleAsync()
        {
            this.BattleLog.Clear();
            if (this.Attacker == null || this.Defender == null || this.Attacker.FastMove == null || this.Attacker.ChargeMove == null || this.Defender.FastMove == null || this.Defender.ChargeMove == null)
            {
                this._messageViewer.DisplayMessage("Pokemon not fully specified", "Invalid Data", MessageViewerButton.Ok, MessageViewerIcon.Warning).Wait();
                return;
            }
            const int timeInterval = 50;
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
                    if (this.DefenderType != DefenderType.GymDefender)
                    {
                        index = (int)this.DefenderType;
                        defenderHP = Constants.RaidBossHP[index - 1];
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
                            if (this.DodgeChargeAttacks && defenderState == BattleState.ChargeAttackWindow)
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
                            attackerDamage = Constants.CalculateDamage(attackerPower, Attacker.GetAttack(), Defender.GetDefense(), attackerBonus);
                            attackerState = BattleState.FastAttackWindow;
                        }
                    }
                    if (attackerState == BattleState.FastAttackWindow)
                    {
                        if (time >= attackerStartAttackTime && time < attackerStartAttackTime + timeInterval)
                        {
                            attackerDamage = Constants.CalculateDamage(attackerPower, Attacker.GetAttack(), Defender.GetDefense(), attackerBonus);
                        }
                        if (time >= attackerStartAttackTime + Attacker.FastMove.FastMove.DamageWindowDuration)
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
                            attackerState = BattleState.ChargeAttackWindow;
                        }
                    }
                    if (attackerState == BattleState.ChargeAttackWindow)
                    {
                        if (time >= attackerStartAttackTime && time < attackerStartAttackTime + timeInterval)
                        {
                            attackerDamage = Constants.CalculateDamage(attackerPower, Attacker.GetAttack(), Defender.GetDefense(), attackerBonus);
                        }
                        if (time >= attackerStartAttackTime + Attacker.ChargeMove.ChargeMove.DamageWindowDuration)
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
                            int randNum = rand.Next(0, 10);
                            if (defenderEnergy >= Defender.ChargeMove.ChargeMove.Energy && (randNum >= 5))
                            {
                                defenderState = BattleState.ChargeAttackInit;
                                defenderStartAttackTime = time + Defender.ChargeMove.ChargeMove.DamageWindowStartTime;
                                defenderNextAttackTime = time + Defender.ChargeMove.ChargeMove.Time;
                            }
                            else if (defenderEnergy < Defender.ChargeMove.ChargeMove.Energy || randNum < 5)
                            {
                                defenderState = BattleState.FastAttackInit;
                                defenderStartAttackTime = time + Defender.FastMove.FastMove.DamageWindowStartTime;
                                defenderNextAttackTime = time + Defender.FastMove.FastMove.Time + rand.Next(Constants.DefenderFastMoveDelayMin, Constants.DefenderFastMoveDelayMax);
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
                            defenderState = BattleState.FastAttackWindow;
                        }
                    }
                    if (defenderState == BattleState.FastAttackWindow)
                    {
                        if (time >= defenderStartAttackTime && time < defenderStartAttackTime + timeInterval)
                        {
                            defenderDamage = Constants.CalculateDamage(defenderPower, Defender.GetAttack(), Attacker.GetDefense(), defenderBonus);
                        }
                        if (time >= defenderStartAttackTime + Defender.FastMove.FastMove.DamageWindowDuration)
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
                            defenderState = BattleState.ChargeAttackWindow;
                        }
                    }
                    if (defenderState == BattleState.ChargeAttackWindow)
                    {
                        if (time >= defenderStartAttackTime && time < defenderStartAttackTime + timeInterval)
                        {
                            defenderDamage = Constants.CalculateDamage(defenderPower, Defender.GetAttack(), Attacker.GetDefense(), defenderBonus);
                        }
                        if (time >= defenderStartAttackTime + Defender.ChargeMove.ChargeMove.DamageWindowDuration)
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

                    if (i == 0)
                    {
                        string attackerAction = "None";
                        string defenderAction = "None";
                        bool recordAction = false;
                        if (attackerDamage > 0)
                        {
                            if (attackerState == BattleState.FastAttackWindow)
                                attackerAction = $"Attacker used {Attacker.FastMove.FastMove.Name}";
                            else if (attackerState == BattleState.ChargeAttackWindow)
                                attackerAction = $"Attacker used {Attacker.ChargeMove.ChargeMove.Name}";
                            recordAction = true;
                        }
                        if (defenderDamage > 0)
                        {
                            if (defenderState == BattleState.FastAttackWindow)
                                defenderAction = $"Defender used {Defender.FastMove.FastMove.Name}";
                            else if (defenderState == BattleState.ChargeAttackWindow)
                                defenderAction = $"Defender used {Defender.ChargeMove.ChargeMove.Name}";
                            recordAction = true;
                        }
                        if (recordAction)
                            this.BattleLog.Add(new BattleLogEntry(time, attackerHP, attackerEnergy, defenderHP, defenderEnergy, attackerAction, defenderAction));
                    }
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
            this.RaisePropertyChanged("BattleLog");
            //Calculate next breakpoint
            double bonus = 1.0;
            if (Attacker.Species.Type1 == Attacker.FastMove.FastMove.Type || Attacker.Species.Type2 == Attacker.FastMove.FastMove.Type)
                bonus *= Constants.StabBonus;
            bonus *= Constants.CalculateTypeBonus(Attacker.FastMove.FastMove.Type, Defender.Species.Type1, Defender.Species.Type2);

            int baseDamage = Constants.CalculateDamage(this.Attacker.FastMove.FastMove.Power, this.Attacker.GetAttack(), this.Defender.GetDefense(), bonus);
            for (double i = Attacker.Level; i <= Constants.MaxLevel; i += 0.5)
            {
                int damage = Constants.CalculateDamage(this.Attacker.FastMove.FastMove.Power, this.Attacker.GetAttack(-1, i), this.Defender.GetDefense(), bonus);
                if (damage > baseDamage)
                {
                    this.NextBreakpoint = i;
                    return;
                }
            }
            this.NextBreakpoint = 0.0;
        }
        #endregion
    }
}
