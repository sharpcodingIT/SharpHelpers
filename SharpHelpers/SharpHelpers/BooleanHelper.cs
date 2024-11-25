// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;

namespace SharpCoding.SharpHelpers
{
    public static class BooleanHelper
    {
        /// <summary>
        /// This method returns TRUE when istance is different from op2
        /// </summary>
        /// <param name="istance"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static bool Xor(this bool istance, bool op2)
        {
            return istance ^ op2;
        }

        /// <summary>
        /// This method returns TRUE when both istance and op2 are true
        /// </summary>
        /// <param name="istance"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static bool And(this bool istance, bool op2)
        {
            return istance && op2;
        }

        /// <summary>
        /// This method returns TRUE when istance that invokes the method or parameter is true.
        /// It returns true when also both are true
        /// </summary>
        /// <param name="istance"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static bool Or(this bool istance, bool op2)
        {
            return istance || op2;
        }

        /// <summary>
        /// This method maps the instance value in one of the two parameters
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        /// <returns></returns>
        public static string ToStringValues(this bool? value, string trueValue, string falseValue)
        {
            return value.HasValue ? (value.Value ? trueValue : falseValue) : falseValue;
        }

        /// <summary>
        /// Negates the instance boolean value.
        /// </summary>
        /// <param name="instance">The boolean value to negate.</param>
        /// <returns>Returns the negated boolean value.</returns>
        public static bool Not(this bool instance)
        {
            return !instance;
        }

        /// <summary>
        /// Determines if the instance is true and executes the specified action if true.
        /// </summary>
        /// <param name="instance">The boolean value to evaluate.</param>
        /// <param name="action">The action to execute if the boolean value is true.</param>
        public static void IfTrue(this bool instance, Action action)
        {
            if (instance)
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// Determines if the instance is false and executes the specified action if false.
        /// </summary>
        /// <param name="instance">The boolean value to evaluate.</param>
        /// <param name="action">The action to execute if the boolean value is false.</param>
        public static void IfFalse(this bool instance, Action action)
        {
            if (!instance)
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// Returns the boolean value as an integer (1 for true, 0 for false).
        /// </summary>
        /// <param name="instance">The boolean value to convert.</param>
        /// <returns>1 if true, 0 if false.</returns>
        public static int ToInt(this bool instance)
        {
            return instance ? 1 : 0;
        }
    }
}
