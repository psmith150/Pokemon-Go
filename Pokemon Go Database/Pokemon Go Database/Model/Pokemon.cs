using System;

namespace Pokemon_Go_Database.Model
{
    [Serializable]
    public class Pokemon
    {
        public string Title
        {
            get;
            private set;
        }

        public Pokemon(string title)
        {
            Title = title;
        }
    }
}