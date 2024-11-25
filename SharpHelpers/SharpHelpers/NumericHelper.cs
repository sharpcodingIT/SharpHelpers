// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;

namespace SharpCoding.SharpHelpers
{
    public static class NumericHelper
    {
        /// <summary>
        /// Checks if the integer number is odd.
        /// </summary>
        /// <param name="numberToCheck">The integer number to check.</param>
        /// <returns>True if the number is odd; otherwise, false.</returns>
        public static bool IsOdd(this int numberToCheck)
        {
            var restOfDivision = (numberToCheck % 2);
            return (restOfDivision == 1);
        }

        /// <summary>
        /// Checks if the integer number is even.
        /// </summary>
        /// <param name="numberToCheck">The integer number to check.</param>
        /// <returns>True if the number is even; otherwise, false.</returns>
        public static bool IsEven(this int numberToCheck)
        {
            return !numberToCheck.IsOdd();
        }

        /// <summary>
        /// Checks if the integer number is prime.
        /// </summary>
        /// <param name="numberToCheck">The integer number to check.</param>
        /// <returns>True if the number is prime; otherwise, false.</returns>
        public static bool IsPrime(this int numberToCheck)
        {
            var limit = Math.Ceiling(Math.Sqrt(numberToCheck));
            for (var i = 2; i <= limit; i++)
            {
                if (numberToCheck % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates the factorial of the integer number.
        /// </summary>
        /// <param name="number">The integer number to calculate the factorial for.</param>
        /// <returns>The factorial of the number.</returns>
        /// <exception cref="ArgumentException">Thrown when the number is negative.</exception>
        public static long Factorial(this int number)
        {
            if (number < 0) throw new ArgumentException("Number must be non-negative.");
            return number <= 1 ? 1 : number * Factorial(number - 1);
        }

        /// <summary>
        /// Computes the Greatest Common Divisor (GCD) of two integers.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The greatest common divisor of the two numbers.</returns>
        public static int GCD(this int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Computes the Least Common Multiple (LCM) of two integers.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <returns>The least common multiple of the two numbers.</returns>
        public static int LCM(this int a, int b)
        {
            return Math.Abs(a * b) / a.GCD(b);
        }

        /// <summary>
        /// Checks if the integer number is negative.
        /// </summary>
        /// <param name="number">The integer number to check.</param>
        /// <returns>True if the number is negative; otherwise, false.</returns>
        public static bool IsNegative(this int number)
        {
            return number < 0;
        }

        /// <summary>
        /// Checks if the integer number is positive.
        /// </summary>
        /// <param name="number">The integer number to check.</param>
        /// <returns>True if the number is positive; otherwise, false.</returns>
        public static bool IsPositive(this int number)
        {
            return number > 0;
        }

        /// <summary>
        /// Clamps the integer number within the specified range.
        /// </summary>
        /// <param name="value">The integer number to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value within the specified range.</returns>
        public static int Clamp(this int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Returns the absolute value of the integer number.
        /// </summary>
        /// <param name="number">The integer number to get the absolute value for.</param>
        /// <returns>The absolute value of the number.</returns>
        public static int Abs(this int number)
        {
            return Math.Abs(number);
        }
    }
}
