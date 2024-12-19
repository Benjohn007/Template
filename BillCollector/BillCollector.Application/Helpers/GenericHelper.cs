using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Application.Helpers
{
    public class GenericHelper
    {
        public static int GenerateRandomInteger(int length = 0)
        {
            var _random = new Random();

            if (length <= 0)
            {
                // If length is not specified or less than or equal to zero, generate any random integer
                return _random.Next();
            }

            // Ensure the length is valid (1 to 9 digits)
            if (length > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length cannot exceed 9 digits when generating random numbers.");
            }

            // Calculate the minimum and maximum range based on the length
            int minValue = (int)Math.Pow(10, length - 1);
            int maxValue = (int)Math.Pow(10, length) - 1;

            return _random.Next(minValue, maxValue + 1);
        }

        public static string GenerateTxnRef()
        {
            Random rn = new Random();
            var numbers = rn.Next(100000, 1000000);
            var dtString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return $"{numbers.ToString()}{dtString}";
        }

        public static string GenerateRandomString(int length)
        {
            var seed = "123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string output = "";
            for (int i = 0; i < length; i++)
            {
                output += seed[new Random().Next(0, seed.Length - 1)];
            }
            return output;
        }
    }
}
