using System;
using System.Collections.Generic;

namespace GameServer.Util
{
    public static class GamertagGenerator
    {
        private static readonly List<string> Adjectives = new()
        {
            "Silly", "Crazy", "Wacky", "Bouncy", "Mighty", "Sneaky", "Flying",
            "Zany", "Giggly", "Quirky", "Lucky", "Spooky"
        };

        private static readonly List<string> Nouns = new()
        {
            "Shoe", "Hat", "Dog", "Car", "Ship", "Thimble", "Unicorn",
            "Dragon", "Cat", "Hammer", "Boot", "Spoon"
        };

        private static readonly Random RandomInstance = new();

        public static string GenerateRandomGamertag()
        {
            var adjective = Adjectives[RandomInstance.Next(Adjectives.Count)];
            var noun = Nouns[RandomInstance.Next(Nouns.Count)];
            var number = RandomInstance.Next(1, 100);

            return $"{adjective}{noun}{number}";
        }
    }
}
