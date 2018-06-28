using System.ComponentModel;

namespace Pokemon_Go_Database.Model
{
    public enum Type {[Description("None")] None, [Description("Normal")] Normal, [Description("Fighting")] Fighting, [Description("Flying")] Flying, [Description("Poison")] Poison, [Description("Ground")] Ground, [Description("Rock")] Rock, [Description("Bug")] Bug, [Description("Ghost")] Ghost, [Description("Steel")] Steel, [Description("Fire")] Fire, [Description("Water")] Water, [Description("Grass")] Grass, [Description("Electric")] Electric, [Description("Psychic")] Psychic, [Description("Ice")] Ice, [Description("Dragon")] Dragon, [Description("Dark")] Dark, [Description("Fairy")] Fairy };
    public enum MoveType { Fast, Charge };
    public enum CombatType { Offense, Defense };
    public enum IVLevel { Low, Medium, High, Max };
    public enum TotalIVLevel { Low, Medium, High, Max};

    public enum DefenderType { [Description("Gym Defender")] GymDefender, [Description("Tier 1")] Tier1, [Description("Tier 2")] Tier2, [Description("Tier 3")] Tier3, [Description("Tier 4")] Tier4, [Description("Tier 5")] Tier5 }
}
