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

        public const double TypeAdvantageBonus = 1.40;

        public const double TypeDisadvantageBonus = 0.714;

        public const double TestDefense = 100.0;

        public const int MaxIV = 15;

        public const double MaxLevel = 40.0;

        public const double DefenseHPBonus = 2.0;

        public static readonly int[] IVLevelCutoffs = {0, 8, 13, 15};

        public static readonly int[] IVSumCutoffs = {0, 23, 30, 37};

        public static readonly double[] DustLevelCutoffs = {1.0, 3.0, 5.0, 7.0, 9.0, 11.0, 13.0, 15.0, 17.0, 19.0, 21.0, 23.0, 25.0, 27.0, 29.0, 31.0, 33.0, 35.0, 37.0, 39.0, 40.0 };

        public static readonly int[] DustCutoffs = { 200, 400, 600, 800, 1000, 1300, 1600, 1900, 2200, 2500, 3000, 3500, 4000, 4500, 5000, 6000, 7000, 8000, 9000, 10000, 11000 };
        public static readonly int[] CandyCutoffs = { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 4, 4, 4, 6, 8, 10, 12, 15, 16 };

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
        }
    }
}
