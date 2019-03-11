using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Go_Database.Model
{
    public static class Constants
    {
        private static readonly double[] _CpmValues = {0.094, 0.16639787, 0.21573247, 0.25572005, 0.29024988,
        0.3210876, 0.34921268, 0.37523559, 0.39956728, 0.42250001,
        0.44310755, 0.46279839, 0.48168495, 0.49985844, 0.51739395,
        0.53435433, 0.55079269, 0.56675452, 0.58227891, 0.59740001,
        0.61215729, 0.62656713, 0.64065295, 0.65443563, 0.667934,
        0.68116492, 0.69414365, 0.70688421, 0.71939909, 0.7317,
        0.73776948, 0.74378943, 0.74976104, 0.75568551, 0.76156384,
        0.76739717, 0.7731865, 0.77893275, 0.78463697, 0.79030001};

        public static readonly double[] CpmValues;

        public const double StabBonus = 1.2;

        public const double IncomingPower = 20.0;

        public const double IncomingAttack = 100.0;

        public const double WeatherBonus = 1.2;

        public const double TypeAdvantageBonus = 1.60;

        public const double TypeAdvantageDoubleBonus = 2.56;

        public const double TypeDisadvantageBonus = .625;

        public const double TypeDisadvantageDoubleBonus = 0.39;

        public const double TestDefense = 100.0;

        public const int MaxIV = 15;

        public const double MaxLevel = 40.0;

        public const double DefenseHPBonus = 2.0;

        public static readonly int[] IVLevelCutoffs = {0, 8, 13, 15};

        public static readonly int[] IVSumCutoffs = {0, 23, 30, 37};

        public static readonly double[] DustLevelCutoffs = {1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0, 23.0, 25.0, 26.0, 27.0, 29.0, 31.0, 33.0, 35.0, 37.0, 39.0, 40.0 };

        public static readonly int[] DustCutoffs = { 200, 400, 600, 800, 1000, 1300, 1600, 1900, 2200, 2500, 3000, 3500, 4000, 4000, 4500, 5000, 6000, 7000, 8000, 9000, 10000, 11000 };
        public static readonly int[] CandyCutoffs = { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4, 6, 8, 10, 12, 15, 16 };

        public static readonly int[] RaidBossHP = { 600, 1800, 3600, 9000, 15000 };

        public static readonly int[] RaidBossLevels = { 20, 25, 30, 40, 40 };

        public const int RaidTimer = 180;

        public const int LegendaryRaidTimer = 300;

        public const int GymTimer = 100;

        public const int PokemonSwitchDelayMs = 1000;

        public const int dodgeDurationMs = 500;

        public const double dodgeDamageFactor = 0.25;

        public const int DefenderFastMoveDelayMin = 1500;

        public const int DefenderFastMoveDelayMax = 2500;

        public const int NumSimulations = 100;

        public const double LuckyStardustMultiplier = 0.5;

        public static string BaseDataFilePath =  AppDomain.CurrentDomain.BaseDirectory + @"BaseData.xml";
        /// <summary>
        /// Attacker,Defender
        /// </summary>
        public static readonly double[,] TypeChart;

        static Constants()
        {
            double[] values = new double[79];
            int tempIndex = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (i % 2 == 0)
                {
                    values[i] = _CpmValues[tempIndex];
                    tempIndex++;
                }
                else
                {
                    values[i] = Math.Sqrt(Math.Pow(_CpmValues[tempIndex - 1], 2) + (Math.Pow(_CpmValues[tempIndex], 2) - Math.Pow(_CpmValues[tempIndex - 1], 2)) / 2.0);
                }
            }
            CpmValues = values;

            TypeChart = new double[Enum.GetNames(typeof(Type)).Length, Enum.GetNames(typeof(Type)).Length];
            for(int i = 0; i < TypeChart.GetLength(0); i++)
            {
                for(int j = 0; j < TypeChart.GetLength(1); j++)
                {
                    TypeChart[i, j] = 1.0;
                }
            }
            //Normal Type
            TypeChart[(int)Type.Normal, (int)Type.Rock] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Normal, (int)Type.Ghost] = TypeDisadvantageDoubleBonus;
            TypeChart[(int)Type.Normal, (int)Type.Steel] = TypeDisadvantageBonus;

            //Fighting Type
            TypeChart[(int)Type.Fighting, (int)Type.Normal] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Flying] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Poison] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Rock] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Bug] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Ghost] = TypeDisadvantageDoubleBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Steel] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Psychic] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Ice] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Dark] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fighting, (int)Type.Fairy] = TypeDisadvantageBonus;

            //Flying Type
            TypeChart[(int)Type.Flying, (int)Type.Fighting] = TypeAdvantageBonus;
            TypeChart[(int)Type.Flying, (int)Type.Rock] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Flying, (int)Type.Bug] = TypeAdvantageBonus;
            TypeChart[(int)Type.Flying, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Flying, (int)Type.Grass] = TypeAdvantageBonus;
            TypeChart[(int)Type.Flying, (int)Type.Electric] = TypeDisadvantageBonus;

            //Poison Type
            TypeChart[(int)Type.Poison, (int)Type.Poison] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Poison, (int)Type.Ground] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Poison, (int)Type.Rock] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Poison, (int)Type.Ghost] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Poison, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Poison, (int)Type.Grass] = TypeAdvantageBonus;
            TypeChart[(int)Type.Poison, (int)Type.Fairy] = TypeAdvantageBonus;

            //Ground Type
            TypeChart[(int)Type.Ground, (int)Type.Flying] = TypeDisadvantageDoubleBonus;
            TypeChart[(int)Type.Ground, (int)Type.Poison] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ground, (int)Type.Rock] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ground, (int)Type.Bug] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Ground, (int)Type.Steel] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ground, (int)Type.Fire] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ground, (int)Type.Grass] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Ground, (int)Type.Electric] = TypeAdvantageBonus;

            //Rock Type
            TypeChart[(int)Type.Rock, (int)Type.Fighting] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Rock, (int)Type.Flying] = TypeAdvantageBonus;
            TypeChart[(int)Type.Rock, (int)Type.Ground] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Rock, (int)Type.Bug] = TypeAdvantageBonus;
            TypeChart[(int)Type.Rock, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Rock, (int)Type.Fire] = TypeAdvantageBonus;
            TypeChart[(int)Type.Rock, (int)Type.Ice] = TypeAdvantageBonus;

            //Bug Type
            TypeChart[(int)Type.Bug, (int)Type.Fighting] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Flying] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Poison] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Ghost] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Fire] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Grass] = TypeAdvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Psychic] = TypeAdvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Dark] = TypeAdvantageBonus;
            TypeChart[(int)Type.Bug, (int)Type.Fairy] = TypeDisadvantageBonus;

            //Ghost Type
            TypeChart[(int)Type.Ghost, (int)Type.Normal] = TypeDisadvantageDoubleBonus;
            TypeChart[(int)Type.Ghost, (int)Type.Ghost] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ghost, (int)Type.Psychic] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ghost, (int)Type.Dark] = TypeDisadvantageBonus;

            //Steel Type
            TypeChart[(int)Type.Steel, (int)Type.Rock] = TypeAdvantageBonus;
            TypeChart[(int)Type.Steel, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Steel, (int)Type.Fire] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Steel, (int)Type.Water] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Steel, (int)Type.Electric] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Steel, (int)Type.Ice] = TypeAdvantageBonus;
            TypeChart[(int)Type.Steel, (int)Type.Fairy] = TypeAdvantageBonus;

            //Fire Type
            TypeChart[(int)Type.Fire, (int)Type.Rock] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Bug] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Steel] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Fire] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Water] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Grass] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Ice] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fire, (int)Type.Dragon] = TypeDisadvantageBonus;

            //Water Type
            TypeChart[(int)Type.Water, (int)Type.Rock] = TypeAdvantageBonus;
            TypeChart[(int)Type.Water, (int)Type.Ground] = TypeAdvantageBonus;
            TypeChart[(int)Type.Water, (int)Type.Fire] = TypeAdvantageBonus;
            TypeChart[(int)Type.Water, (int)Type.Water] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Water, (int)Type.Grass] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Water, (int)Type.Dragon] = TypeDisadvantageBonus;

            //Grass Type
            TypeChart[(int)Type.Grass, (int)Type.Flying] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Poison] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Ground] = TypeAdvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Rock] = TypeAdvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Bug] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Fire] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Water] = TypeAdvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Grass] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Grass, (int)Type.Dragon] = TypeDisadvantageBonus;

            //Electric Type
            TypeChart[(int)Type.Electric, (int)Type.Flying] = TypeAdvantageBonus;
            TypeChart[(int)Type.Electric, (int)Type.Ground] = TypeDisadvantageDoubleBonus;
            TypeChart[(int)Type.Electric, (int)Type.Water] = TypeAdvantageBonus;
            TypeChart[(int)Type.Electric, (int)Type.Grass] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Electric, (int)Type.Electric] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Electric, (int)Type.Dragon] = TypeDisadvantageBonus;

            //Psychic Type
            TypeChart[(int)Type.Psychic, (int)Type.Fighting] = TypeAdvantageBonus;
            TypeChart[(int)Type.Psychic, (int)Type.Poison] = TypeAdvantageBonus;
            TypeChart[(int)Type.Psychic, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Psychic, (int)Type.Psychic] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Psychic, (int)Type.Dark] = TypeDisadvantageDoubleBonus;

            //Ice Type
            TypeChart[(int)Type.Ice, (int)Type.Flying] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Ground] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Fire] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Water] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Grass] = TypeAdvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Ice] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Ice, (int)Type.Dragon] = TypeAdvantageBonus;

            //Dragon Type
            TypeChart[(int)Type.Dragon, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Dragon, (int)Type.Dragon] = TypeAdvantageBonus;
            TypeChart[(int)Type.Dragon, (int)Type.Fairy] = TypeDisadvantageDoubleBonus;

            //Dark Type
            TypeChart[(int)Type.Dark, (int)Type.Fighting] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Dark, (int)Type.Ghost] = TypeAdvantageBonus;
            TypeChart[(int)Type.Dark, (int)Type.Psychic] = TypeAdvantageBonus;
            TypeChart[(int)Type.Dark, (int)Type.Dark] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Dark, (int)Type.Fairy] = TypeDisadvantageBonus;

            //Fairy Type
            TypeChart[(int)Type.Fairy, (int)Type.Fighting] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fairy, (int)Type.Poison] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fairy, (int)Type.Steel] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fairy, (int)Type.Fire] = TypeDisadvantageBonus;
            TypeChart[(int)Type.Fairy, (int)Type.Dragon] = TypeAdvantageBonus;
            TypeChart[(int)Type.Fairy, (int)Type.Dark] = TypeAdvantageBonus;
        }

        public static int CalculateDamage(int power, double attack, double defense, double bonus)
        {
            return (int)Math.Floor(0.5 * power * attack * bonus / defense) + 1;
        }

        public static double CalculateTypeBonus(Type moveType, Type pokemonType1, Type pokemonType2)
        {
            return TypeChart[(int)moveType, (int)pokemonType1] * TypeChart[(int)moveType, (int)pokemonType2];
        }
        public static double CalculateWeatherBonus(Type moveType, Weather weather)
        {
            double bonus = 1.0;
            switch (weather)
            {
                case Weather.SunnyClear:
                    if (moveType == Type.Grass || moveType == Type.Ground || moveType == Type.Fire)
                        bonus = WeatherBonus;
                    break;
                case Weather.Rain:
                    if (moveType == Type.Water || moveType == Type.Electric || moveType == Type.Bug)
                        bonus = WeatherBonus;
                    break;
                case Weather.Wind:
                    if (moveType == Type.Dragon || moveType == Type.Flying || moveType == Type.Psychic)
                        bonus = WeatherBonus;
                    break;
                case Weather.Snow:
                    if (moveType == Type.Ice || moveType == Type.Steel)
                        bonus = WeatherBonus;
                    break;
                case Weather.Fog:
                    if (moveType == Type.Dark || moveType == Type.Ghost)
                        bonus = WeatherBonus;
                    break;
                case Weather.Cloudy:
                    if (moveType == Type.Fairy || moveType == Type.Fighting || moveType == Type.Poison)
                        bonus = WeatherBonus;
                    break;
                case Weather.PartlyCloudy:
                    if (moveType == Type.Normal || moveType == Type.Rock)
                        bonus = WeatherBonus;
                    break;
                default:
                    break;
            }
            return bonus;
        }
    }
}
