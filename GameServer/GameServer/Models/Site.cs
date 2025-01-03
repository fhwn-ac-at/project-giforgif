namespace GameServer.Models
{
    public abstract class Site : IProperty
    {
        public string Name { get; set; }
        public int MortgagePrice { get; set; }
        public int NormalPrice { get; set; }
        // Rent Prices for 0, 1, 2, 3, 4 houses and hotel
        public int[] RentPrices { get; set; }
    }
}
