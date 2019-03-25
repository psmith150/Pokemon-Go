using GalaSoft.MvvmLight.Command;
using Pokemon_Go_Database.Base.AbstractClasses;
using Pokemon_Go_Database.Base.Enums;
using Pokemon_Go_Database.Model;
using Pokemon_Go_Database.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ICommand SimulateAllPokemonCommand { get; private set; }
        public ICommand IncrementPartySizeCommand { get; private set; }
        public ICommand DecrementPartySizeCommand { get; private set; }
        public ICommand AssignAttackerPokemonCommand { get; private set; }
        #endregion

        #region Constructor
        public BattleSimulationViewModel(NavigationService navigationService, SessionService session, MessageViewerBase messageViewer) : base(session)
        {
            this.navigationService = navigationService;
            this._messageViewer = messageViewer;
            this.Attackers = new ObservableCollection<AttackerSimulationWrapper>();
            this.Defender = new Pokemon();
            this.BattleLog = new MyObservableCollection<BattleLogEntry>();
            this.AllPokemonResults = new MyObservableCollection<BattleResult>();

            this.SimulateBattleCommand = new RelayCommand(() => this.SimulateSingleBattle());
            this.SimulateAllPokemonCommand = new RelayCommand(() => this.SimulateAllPokemon());
            this.IncrementPartySizeCommand = new RelayCommand(() => this.IncrementPartySize());
            this.DecrementPartySizeCommand = new RelayCommand(() => this.DecrementPartySize());
            this.AssignAttackerPokemonCommand = new RelayCommand<int>((position) => this.AssignSelectedPokemonToParty(position));

            this._partySize = 1;
            Pokemon newPokemon = new Pokemon();
            newPokemon.IVSets.Add(new IVSet());
            this.Attackers.Add(new AttackerSimulationWrapper(newPokemon, new BattleResult()));
            this.UpdatePartyVisibility();
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
        private int _partySize;
        private const int MAX_PARTY_SIZE = 6;
        #endregion
        #region Public Properties
        private ObservableCollection<AttackerSimulationWrapper> _Attackers;
        public ObservableCollection<AttackerSimulationWrapper> Attackers
        {
            get
            {
                return _Attackers;
            }

            private set
            {
                this.Set(ref this._Attackers, value);
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
                //if (this.SelectedAttackerPokemon != null)
                //    this.Attacker = this.SelectedAttackerPokemon.Copy();
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
        private MyObservableCollection<BattleLogEntry> _BattleLog;
        public MyObservableCollection<BattleLogEntry> BattleLog
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

        private MyObservableCollection<BattleResult> _AllPokemonResults;
        public MyObservableCollection<BattleResult> AllPokemonResults
        {
            get
            {
                return this._AllPokemonResults;
            }
            set
            {
                this.Set(ref this._AllPokemonResults, value);
            }
        }
        public Array WeatherOptions
        {
            get
            {
                return Enum.GetValues(typeof(Weather));
            }
        }
        private Weather _SelectedWeather;
        public Weather SelectedWeather
        {
            get
            {
                return this._SelectedWeather;
            }
            set
            {
                this.Set(ref this._SelectedWeather, value);
            }
        }

        public Array FriendshipOptions
        {
            get
            {
                return Enum.GetValues(typeof(Friendship));
            }
        }

        private Friendship _SelectedFriendship;
        public Friendship SelectedFriendship
        {
            get
            {
                return this._SelectedFriendship;
            }
            set
            {
                this.Set(ref this._SelectedFriendship, value);
            }
        }
        #region Party Visbility
        private bool _IncrementPartyButtonVisible;
        public bool IncrementPartyButtonVisible
        {
            get
            {
                return this._IncrementPartyButtonVisible;
            }
            set
            {
                this.Set(ref this._IncrementPartyButtonVisible, value);
            }
        }
        private bool _DecrementPartyButtonVisible;
        public bool DecrementPartyButtonVisible
        {
            get
            {
                return this._DecrementPartyButtonVisible;
            }
            set
            {
                this.Set(ref this._DecrementPartyButtonVisible, value);
            }
        }
        #endregion
        #endregion

        #region Private Methods
        private void UpdatePartyVisibility()
        {
            //Set increment/decrement button visbility
            if (this._partySize <= 1)
                this.DecrementPartyButtonVisible = false;
            else
                this.DecrementPartyButtonVisible = true;
            if (this._partySize >= MAX_PARTY_SIZE)
                this.IncrementPartyButtonVisible = false;
            else
                this.IncrementPartyButtonVisible = true;
            //Limit the party size
            if (this._partySize < 1)
            {
                this._partySize = 1;
                this.UpdatePartyVisibility();
            }
            if (this._partySize > MAX_PARTY_SIZE)
            {
                this._partySize = MAX_PARTY_SIZE;
                this.UpdatePartyVisibility();
            }
        }
        private void IncrementPartySize()
        {
            if (this._partySize >= MAX_PARTY_SIZE)
            {
                this._partySize = MAX_PARTY_SIZE;
            }
            else
            {
                this._partySize += 1;
                Pokemon newPokemon = new Pokemon();
                newPokemon.IVSets.Add(new IVSet());
                this.Attackers.Add(new AttackerSimulationWrapper(newPokemon, new BattleResult()));
            }
            List<string> speciesNames = new List<string>(this.Attackers.Select(x => x.Attacker.Species.Species));
            this.UpdatePartyVisibility();
        }
        private void DecrementPartySize()
        {
            if (this._partySize <= 1)
            {
                this._partySize = 1;
            }
            else
            {
                this._partySize -= 1;
                this.Attackers.RemoveAt(this.Attackers.Count - 1);
            }
            this.UpdatePartyVisibility();
        }
        private void AssignSelectedPokemonToParty(int position)
        {
            if (position < 0 || position >= this._partySize || this.SelectedAttackerPokemon == null)
                return;
            this.Attackers[position].Attacker = this.SelectedAttackerPokemon.Copy();
        }
        private async void SimulateSingleBattle()
        {
            List<BattleLogEntry> battleLog = new List<BattleLogEntry>();
            try
            {
                List<AttackerSimulationWrapper> attackers = this.Attackers.ToList();
                battleLog = await Task.Run(() => this.SimulateBattleAsync(attackers, this.Defender, this.DefenderType));
            }
            catch (Exception ex)
            {
                await this._messageViewer.DisplayMessage($"Error when simulating battle: {ex.Message}", "Simulation Error", MessageViewerButton.Ok, MessageViewerIcon.Error);
            }
            this.BattleLog.Clear();
            this.BattleLog.InsertRange(battleLog);
        }

        private async void SimulateAllPokemon()
        {
            if (this.Defender == null || this.Defender.FastMove == null || this.Defender.ChargeMove == null)
            {
                await this._messageViewer.DisplayMessage("Pokemon not fully specified", "Invalid Data", MessageViewerButton.Ok, MessageViewerIcon.Warning);
                this.AllPokemonResults.Clear();
                return;
            }
            List<BattleResult> results = new List<BattleResult>();
            foreach (Pokemon pokemon in this.Session.MyPokemon)
            {
                List<AttackerSimulationWrapper> attackers = new List<AttackerSimulationWrapper>();
                attackers.Add(new AttackerSimulationWrapper(pokemon, new BattleResult()));
                try
                {
                    await Task.Run(() => this.SimulateBattleAsync(attackers, this.Defender, this.DefenderType));
                    results.Add(attackers[0].BattleResult);
                }
                catch (Exception ex)
                {
                    await this._messageViewer.DisplayMessage($"Error when simulating battle: {ex.Message}", "Simulation Error", MessageViewerButton.Ok, MessageViewerIcon.Error);
                }
            }
            this.AllPokemonResults.Clear();
            this.AllPokemonResults.InsertRange(results);
        }
        private List<BattleLogEntry> SimulateBattleAsync(List<AttackerSimulationWrapper> attackers, Pokemon defender, DefenderType defenderType)
        {
            List<BattleLogEntry> battleLog = new List<BattleLogEntry>();
            foreach (AttackerSimulationWrapper wrapper in attackers)
            {
                Pokemon attacker = wrapper.Attacker;
                BattleResult result = wrapper.BattleResult;
                result.Name = attacker.FullName;
                if (attacker == null || defender == null || attacker.FastMove == null || attacker.ChargeMove == null || defender.FastMove == null || defender.ChargeMove == null)
                {
                    throw new ArgumentException("Pokemon not fully specified");
                }
            }
            const int timeInterval = 50;
            int attackerLivesNeeded = 0;
            Random rand = new Random();
            for (int i = 0; i < Constants.NumSimulations; i++)
            {
                int attackerIndex = 0;
                Pokemon attacker = attackers[attackerIndex].Attacker;
                BattleResult result = attackers[attackerIndex].BattleResult;
                attackerLivesNeeded = 0;
                int attackerHP = attacker.ActualHP;
                int defenderHP = defender.ActualHP * 2;
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
                    if (defenderType != DefenderType.GymDefender)
                    {
                        index = (int)defenderType;
                        defenderHP = Constants.RaidBossHP[index - 1];
                    }
                }
                int defenderTotalHP = defenderHP;
                int defenderStartHP = defenderHP;
                int startTime = 0;
                int time = 0; //Time in msec
                double timeRemaining;
                BattleState attackerState = BattleState.Idle;
                BattleState defenderState = BattleState.Idle;
                int attackerPower = 0, defenderPower = 0;
                double attackerBonus = 1.0, defenderBonus = 1.0;
                while (attackerIndex < attackers.Count && time < 1000000)
                {
                    int attackerDamage = 0, defenderDamage = 0;
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
                            else if (attackerEnergy >= attacker.ChargeMove.ChargeMove.Energy)
                            {
                                attackerState = BattleState.ChargeAttackInit;
                                attackerStartAttackTime = time + attacker.ChargeMove.ChargeMove.DamageWindowStartTime;
                                attackerNextAttackTime = time + attacker.ChargeMove.ChargeMove.Time;
                            }
                            else if (attackerEnergy < attacker.ChargeMove.ChargeMove.Energy)
                            {
                                attackerState = BattleState.FastAttackInit;
                                attackerStartAttackTime = time + attacker.FastMove.FastMove.DamageWindowStartTime;
                                attackerNextAttackTime = time + attacker.FastMove.FastMove.Time;
                            }
                        }
                    }
                    if (attackerState == BattleState.FastAttackInit)
                    {
                        if (time >= attackerStartAttackTime)
                        {
                            attackerPower = attacker.FastMove.FastMove.Power;
                            if (attacker.Species.Type1 == attacker.FastMove.FastMove.Type || attacker.Species.Type2 == attacker.FastMove.FastMove.Type)
                                attackerBonus *= Constants.StabBonus;
                            attackerBonus *= Constants.CalculateWeatherBonus(attacker.FastMove.FastMove.Type, this.SelectedWeather);
                            attackerBonus *= Constants.CalculateTypeBonus(attacker.FastMove.FastMove.Type, defender.Species.Type1, defender.Species.Type2);
                            attackerBonus *= Constants.CalculateFriendshipBonus(this.SelectedFriendship);
                            attackerEnergy += attacker.FastMove.FastMove.Energy;
                            attackerDamage = Constants.CalculateDamage(attackerPower, attacker.GetAttack(), defender.GetDefense(), attackerBonus);
                            attackerState = BattleState.FastAttackWindow;
                        }
                    }
                    if (attackerState == BattleState.FastAttackWindow)
                    {
                        if (time >= attackerStartAttackTime && time < attackerStartAttackTime + timeInterval)
                        {
                            attackerDamage = Constants.CalculateDamage(attackerPower, attacker.GetAttack(), defender.GetDefense(), attackerBonus);
                        }
                        if (time >= attackerStartAttackTime + attacker.FastMove.FastMove.DamageWindowDuration)
                        {
                            attackerState = BattleState.Idle;
                        }
                    }
                    if (attackerState == BattleState.ChargeAttackInit)
                    {
                        if (time >= attackerStartAttackTime)
                        {
                            attackerPower = attacker.ChargeMove.ChargeMove.Power;
                            attackerEnergy -= attacker.ChargeMove.ChargeMove.Energy;
                            if (attacker.Species.Type1 == attacker.ChargeMove.ChargeMove.Type || attacker.Species.Type2 == attacker.ChargeMove.ChargeMove.Type)
                                attackerBonus *= Constants.StabBonus;
                            attackerBonus *= Constants.CalculateWeatherBonus(attacker.ChargeMove.ChargeMove.Type, this.SelectedWeather);
                            attackerBonus *= Constants.CalculateTypeBonus(attacker.ChargeMove.ChargeMove.Type, defender.Species.Type1, defender.Species.Type2);
                            attackerBonus *= Constants.CalculateFriendshipBonus(this.SelectedFriendship);
                            attackerState = BattleState.ChargeAttackWindow;
                        }
                    }
                    if (attackerState == BattleState.ChargeAttackWindow)
                    {
                        if (time >= attackerStartAttackTime && time < attackerStartAttackTime + timeInterval)
                        {
                            attackerDamage = Constants.CalculateDamage(attackerPower, attacker.GetAttack(), defender.GetDefense(), attackerBonus);
                        }
                        if (time >= attackerStartAttackTime + attacker.ChargeMove.ChargeMove.DamageWindowDuration)
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
                            if (defenderEnergy >= defender.ChargeMove.ChargeMove.Energy && (randNum >= 5))
                            {
                                defenderState = BattleState.ChargeAttackInit;
                                defenderStartAttackTime = time + defender.ChargeMove.ChargeMove.DamageWindowStartTime;
                                defenderNextAttackTime = time + defender.ChargeMove.ChargeMove.Time + rand.Next(Constants.DefenderFastMoveDelayMin, Constants.DefenderFastMoveDelayMax); ;
                            }
                            else if (defenderEnergy < defender.ChargeMove.ChargeMove.Energy || randNum < 5)
                            {
                                defenderState = BattleState.FastAttackInit;
                                defenderStartAttackTime = time + defender.FastMove.FastMove.DamageWindowStartTime;
                                defenderNextAttackTime = time + defender.FastMove.FastMove.Time + rand.Next(Constants.DefenderFastMoveDelayMin, Constants.DefenderFastMoveDelayMax);
                            }
                        }
                    }
                    if (defenderState == BattleState.FastAttackInit)
                    {
                        if (time >= defenderStartAttackTime)
                        {
                            defenderPower = defender.FastMove.FastMove.Power;
                            if (defender.Species.Type1 == defender.FastMove.FastMove.Type || defender.Species.Type2 == defender.FastMove.FastMove.Type)
                                defenderBonus *= Constants.StabBonus;
                            defenderBonus *= Constants.CalculateWeatherBonus(defender.FastMove.FastMove.Type, this.SelectedWeather);
                            defenderBonus *= Constants.CalculateTypeBonus(defender.FastMove.FastMove.Type, attacker.Species.Type1, attacker.Species.Type2);
                            defenderEnergy += defender.FastMove.FastMove.Energy;
                            defenderState = BattleState.FastAttackWindow;
                        }
                    }
                    if (defenderState == BattleState.FastAttackWindow)
                    {
                        if (time >= defenderStartAttackTime && time < defenderStartAttackTime + timeInterval)
                        {
                            defenderDamage = Constants.CalculateDamage(defenderPower, defender.GetAttack(), attacker.GetDefense(), defenderBonus);
                        }
                        if (time >= defenderStartAttackTime + defender.FastMove.FastMove.DamageWindowDuration)
                        {
                            defenderState = BattleState.Idle;
                        }
                    }
                    if (defenderState == BattleState.ChargeAttackInit)
                    {
                        if (time >= defenderStartAttackTime)
                        {
                            defenderPower = defender.ChargeMove.ChargeMove.Power;
                            defenderEnergy -= defender.ChargeMove.ChargeMove.Energy;
                            if (defender.Species.Type1 == defender.ChargeMove.ChargeMove.Type || defender.Species.Type2 == defender.ChargeMove.ChargeMove.Type)
                                defenderBonus *= Constants.StabBonus;
                            defenderBonus *= Constants.CalculateWeatherBonus(defender.ChargeMove.ChargeMove.Type, this.SelectedWeather);
                            defenderBonus *= Constants.CalculateTypeBonus(defender.ChargeMove.ChargeMove.Type, attacker.Species.Type1, attacker.Species.Type2);
                            defenderState = BattleState.ChargeAttackWindow;
                        }
                    }
                    if (defenderState == BattleState.ChargeAttackWindow)
                    {
                        if (time >= defenderStartAttackTime && time < defenderStartAttackTime + timeInterval)
                        {
                            defenderDamage = Constants.CalculateDamage(defenderPower, defender.GetAttack(), attacker.GetDefense(), defenderBonus);
                        }
                        if (time >= defenderStartAttackTime + defender.ChargeMove.ChargeMove.DamageWindowDuration)
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
                                attackerAction = $"{attacker.FullName} used {attacker.FastMove.FastMove.Name}";
                            else if (attackerState == BattleState.ChargeAttackWindow)
                                attackerAction = $"{attacker.FullName} used {attacker.ChargeMove.ChargeMove.Name}";
                            recordAction = true;
                        }
                        if (defenderDamage > 0)
                        {
                            if (defenderState == BattleState.FastAttackWindow)
                                defenderAction = $"{defender.Species.Species} used {defender.FastMove.FastMove.Name}";
                            else if (defenderState == BattleState.ChargeAttackWindow)
                                defenderAction = $"{defender.Species.Species} used {defender.ChargeMove.ChargeMove.Name}";
                            recordAction = true;
                        }
                        if (recordAction)
                            battleLog.Add(new BattleLogEntry(time, attackerHP, attackerEnergy, defenderHP, defenderEnergy, attackerAction, defenderAction));
                    }
                    if (attackerHP <= 0)
                    {
                        if (i == 0)
                            battleLog.Add(new BattleLogEntry(time, attackerHP, attackerEnergy, defenderHP, defenderEnergy, $"{attacker.FullName} fainted", "None"));

                        result.DPS += (double)(defenderStartHP - defenderHP) / ((double)(time - timeInterval - startTime) / 1000.0);
                        result.TDO += (double)(defenderStartHP - defenderHP) / (double)defenderTotalHP;
                        result.BattleDuration += (time - timeInterval - startTime) / 1000.0;
                        result.NumberOfDeaths += attackerLivesNeeded;

                        result.CumulativeDPS += (double)(defenderTotalHP - defenderHP) / ((double)(time - timeInterval) / 1000.0);
                        result.CumulativeTDO += (double)(defenderTotalHP - defenderHP) / (double)defenderTotalHP;
                        result.CumulativeDuration += (time - timeInterval) / 1000.0;
                        attackerIndex++;
                        if (attackerIndex >= attackers.Count)
                            break;
                        attacker = attackers[attackerIndex].Attacker;
                        attackerHP = attacker.ActualHP;
                        attackerEnergy = 0;
                        attackerNextAttackTime = time + Constants.PokemonSwitchDelayMs;
                        attackerState = BattleState.Idle;
                        defenderStartHP = defenderHP;
                        startTime = time - timeInterval;
                        result = attackers[attackerIndex].BattleResult;
                    }
                    if (defenderHP <= 0)
                    {
                        if (i == 0)
                            battleLog.Add(new BattleLogEntry(time, attackerHP, attackerEnergy, defenderHP, defenderEnergy, $"{attacker.FullName} wins!", $"{defender.Species.Species} fainted"));

                        result.DPS += (double)(defenderStartHP - defenderHP) / ((double)(time - timeInterval) / 1000.0);
                        result.TDO += (double)(defenderStartHP - 0) / (double)defenderTotalHP;
                        result.BattleDuration += (time - timeInterval - startTime) / 1000.0;

                        result.CumulativeDPS += (double)(defenderTotalHP - defenderHP) / ((double)(time - timeInterval) / 1000.0);
                        result.CumulativeTDO += (double)(defenderTotalHP - 0) / (double)defenderTotalHP;
                        result.CumulativeDuration += (time - timeInterval) / 1000.0;
                        break;
                    }
                    time += timeInterval;
                    if (this.IsRaidBoss)
                        timeRemaining = Constants.RaidTimer - time / 1000.0;
                    else
                        timeRemaining = Constants.GymTimer - time / 1000.0;
                }
            }
            foreach (AttackerSimulationWrapper wrapper in attackers)
            {
                BattleResult result = wrapper.BattleResult;
                result.BattleDuration = result.BattleDuration / (double)Constants.NumSimulations;
                result.NumberOfDeaths = result.NumberOfDeaths / (double)Constants.NumSimulations;
                result.DPS = result.DPS / (double)Constants.NumSimulations;
                result.TDO = result.TDO / (double)Constants.NumSimulations;
                result.CumulativeDuration = result.CumulativeDuration / (double)Constants.NumSimulations;
                result.CumulativeDPS = result.CumulativeDPS / (double)Constants.NumSimulations;
                result.CumulativeTDO = result.CumulativeTDO / (double)Constants.NumSimulations;

                //Calculate next breakpoint
                Pokemon attacker = wrapper.Attacker;
                double bonus = 1.0;
                if (attacker.Species.Type1 == attacker.FastMove.FastMove.Type || attacker.Species.Type2 == attacker.FastMove.FastMove.Type)
                    bonus *= Constants.StabBonus;
                bonus *= Constants.CalculateTypeBonus(attacker.FastMove.FastMove.Type, defender.Species.Type1, defender.Species.Type2);
                bonus *= Constants.CalculateWeatherBonus(attacker.FastMove.FastMove.Type, this.SelectedWeather);
                bonus *= Constants.CalculateFriendshipBonus(this.SelectedFriendship);

                int baseDamage = Constants.CalculateDamage(attacker.FastMove.FastMove.Power, attacker.GetAttack(), defender.GetDefense(), bonus);
                for (double i = attacker.Level; i <= Constants.MaxLevel; i += 0.5)
                {
                    int damage = Constants.CalculateDamage(attacker.FastMove.FastMove.Power, attacker.GetAttack(-1, i), defender.GetDefense(), bonus);
                    if (damage > baseDamage)
                    {
                        result.NextBreakpoint = i;
                        break;
                    }
                }
            }
            return battleLog;
        }
        #endregion
    }
}
