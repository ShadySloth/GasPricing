
namespace Tests.Util;

public class TestDataGenerator
{
    public static IEnumerable<object[]> GetValidData()
    {
        var data = new List<object[]>
        {
            new object[] { "Diesel", 1, false, 7.2 },
            new object[] { "Regular", 1, false, 12.7 },
            new object[] { "Premium", 1, false, 14.5 },
            new object[] { "Diesel", 19, false, 136.8 },
            new object[] { "Regular", 19, false, 241.3 },
            new object[] { "Premium", 19, false, 275.5 },
            new object[] { "Diesel", 20, false, 144 },
            new object[] { "Regular", 20, false, 254 },
            new object[] { "Premium", 20, false, 290 },
            new object[] { "Diesel", 21, false, 143.64 },
            new object[] { "Regular", 21, false, 253.37 },
            new object[] { "Premium", 21, false, 289.28 },
            new object[] { "Diesel", 49, false, 335.16 },
            new object[] { "Regular", 49, false, 591.19 },
            new object[] { "Premium", 49, false, 674.98 },
            new object[] { "Diesel", 50, false, 342 },
            new object[] { "Regular", 50, false, 603.25 },
            new object[] { "Premium", 50, false, 688.75 },
            new object[] { "Diesel", 51, false, 330.48 },
            new object[] { "Regular", 51, false, 582.93 },
            new object[] { "Premium", 51, false, 665.55 },
            new object[] { "Diesel", 99, false, 641.52 },
            new object[] { "Regular", 99, false, 1_131.57 },
            new object[] { "Premium", 99, false, 1_291.95 },
            new object[] { "Diesel", 100, false, 648 },
            new object[] { "Regular", 100, false, 1_143 },
            new object[] { "Premium", 100, false, 1_305 },
            new object[] { "Diesel", 101, false, 618.12 },
            new object[] { "Regular", 101, false, 1_090.30 },
            new object[] { "Premium", 101, false, 1_244.83 },
            new object[] { "Diesel", 15, true, 97.2 },
            new object[] { "Diesel", 35, true, 214.2 },
            new object[] { "Diesel", 65, true, 374.4 },
            new object[] { "Diesel", 105, true, 567 }
        };

        return data;
    }
    
    public static IEnumerable<object[]> GetDataWithWrongLiters()
    {
        var data = new List<object[]>
        {
            new object[] { "Diesel", -1, false },
            new object[] { "Regular", -1, false },
            new object[] { "Premium", -1, false },
            new object[] { "Diesel", -1, true },
            new object[] { "Regular", -1, true },
            new object[] { "Premium", -1, true },
            new object[] { "Diesel", 0, false },
            new object[] { "Regular", 0, false },
            new object[] { "Premium", 0, false },
            new object[] { "Diesel", 0, true },
            new object[] { "Regular", 0, true },
            new object[] { "Premium", 0, true }
        };

        return data;
    }

    public static IEnumerable<object[]> GetDataWithWrongGasType()
    {
        var data = new List<object[]>
        {
            new object[] { "Milk", 45, false }
        };

        return data;
    }

    public static IEnumerable<object[]> GetDataWithNullGasType()
    {
        var data = new List<object[]>
        {
            new object[] { null!, 10, false }
        };

        return data;
    }
}
