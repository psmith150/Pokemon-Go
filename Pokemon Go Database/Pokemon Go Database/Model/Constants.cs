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

        public const int MaxIV = 15;

        public const double MaxLevel = 40.0;

        static Constants()
        {
            double[] values = new double[78];
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
