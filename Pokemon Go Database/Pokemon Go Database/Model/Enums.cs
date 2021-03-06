﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Pokemon_Go_Database.Model
{
    public enum Type
    {
        [Display(Description = "None")] None,
        [Display(Description = "Normal")] Normal,
        [Display(Description = "Fighting")] Fighting,
        [Display(Description = "Flying")] Flying,
        [Display(Description = "Poison")] Poison,
        [Display(Description = "Ground")] Ground,
        [Display(Description = "Rock")] Rock,
        [Display(Description = "Bug")] Bug,
        [Display(Description = "Ghost")] Ghost,
        [Display(Description = "Steel")] Steel,
        [Display(Description = "Fire")] Fire,
        [Display(Description = "Water")] Water,
        [Display(Description = "Grass")] Grass,
        [Display(Description = "Electric")] Electric,
        [Display(Description = "Psychic")] Psychic,
        [Display(Description = "Ice")] Ice,
        [Display(Description = "Dragon")] Dragon,
        [Display(Description = "Dark")] Dark,
        [Display(Description = "Fairy")] Fairy
    };
    public enum MoveType
    {
        [Display(Description = "Fast Move")] Fast,
        [Display(Description = "Charge Move")] Charge
    };
    public enum CombatType
    {
        [Display(Description = "Offense")] Offense,
        [Display(Description = "Defense")] Defense
    };
    public enum IVLevel
    {
        [Display(Description = "Low (0-7)")] Low,
        [Display(Description = "Medium (8-12)")] Medium,
        [Display(Description = "High (13-14)")] High,
        [Display(Description = "Max (15)")] Max
    };
    public enum TotalIVLevel
    {
        [Display(Description = "Below Average")] Low,
        [Display(Description = "Above Average")] Medium,
        [Display(Description = "Great")] High,
        [Display(Description = "Excellent")] Max
    };

    public enum DefenderType
    {
        [Display(Description = "Gym Defender")] GymDefender,
        [Display(Description = "Tier 1")] Tier1,
        [Display(Description = "Tier 2")] Tier2,
        [Display(Description = "Tier 3")] Tier3,
        [Display(Description = "Tier 4")] Tier4,
        [Display(Description = "Tier 5")] Tier5
    };

    public enum Weather
    {
        [Display(Description = "Extreme")] Extreme,
        [Display(Description = "Sunny/Clear")] SunnyClear,
        [Display(Description = "Rain")] Rain,
        [Display(Description = "Wind")] Wind,
        [Display(Description = "Snow")] Snow,
        [Display(Description = "Fog")] Fog,
        [Display(Description = "Cloudy")] Cloudy,
        [Display(Description = "Partly Cloudy")] PartlyCloudy
    };

    public enum Friendship
    {
        [Display(Description = "None")] None,
        [Display(Description = "Good Friends")] Good,
        [Display(Description = "Great Friends")] Great,
        [Display(Description = "Ultra Friends")] Ultra,
        [Display(Description = "Best Friends")] Best
    }

    public enum PokemonFilterType
    {
        [Display(Description = "Name")] Name,
        [Display(Description = "Species")] Species,
        [Display(Description = "Move")] Move,
        [Display(Description = "Move Type")] MoveType,
        [Display(Description = "Type")] Type,
        [Display(Description = "CP")] CP,
        [Display(Description = "HP")] HP,
        [Display(Description = "Attack IV")] Attack,
        [Display(Description = "Defense IV")] Defense,
        [Display(Description = "Stamina IV")] Stamina,
        [Display(Description = "Level")] Level,
        [Display(Description = "IV %")] IVPercent,
        [Display(Description = "Pokedex #")] PokedexNum
    }
    public enum PokedexFilterType
    {
        [Display(Description = "Name")] Name,
        [Display(Description = "Pokedex #")] PokedexNum,
        [Display(Description = "Move")] Move,
        [Display(Description = "Type")] Type,
        [Display(Description = "Attack")] BaseAttack,
        [Display(Description = "Defense")] BaseDefense,
        [Display(Description = "Stamina")] BaseStamina,
        [Display(Description = "Max CP")] MaxCP,
        [Display(Description = "Total Stats")] BaseStatTotal
    }

    public enum FilterComparisonType
    {
        [Display(Description = "==")] Equal,
        [Display(Description = ">=")] GreaterThanOrEqual,
        [Display(Description = "<=")] LessThanOrEqual,
        [Display(Description = ">")] GreaterThan,
        [Display(Description = "<")] LessThan,
        [Display(Description = "!=")] NotEqual
    }
}
