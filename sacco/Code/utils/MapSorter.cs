// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
/// Allows to easily sort a dictionary by its keys - ported by Dart that manages Map instead
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace commercio.sacco.lib
{
    public class MapSorter
    {
        #region Instance Variables


        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods

        /// Takes the given Dictionary and orders its values based on their keys.
        /// Returns the sorted Dictionary.
        /// Quite different from te original Dart implementation
        public static Dictionary<String, Object> sort(Dictionary<String, Object> map)
        {
            Object wkValue;

            // Get the sorted keys
            List<String> wkList = new List<String>(map.Keys);
            wkList.Sort();

            Dictionary<String, Object> result = new Dictionary<String, Object>();
            // Sort each value
            foreach (String item in wkList)
            {
                if (map.TryGetValue(item, out wkValue))
                {
                    result.Add(item, orderValue(wkValue));
                }
            }
            return result;
        }

        #endregion

        #region Helpers

        /// Takes the given [value] and orders each one of the contained
        /// items that are present inside it by calling [orderValue].
        private static List<Object> orderList(List<Object> value)
        {
            List<Object> result = new List<Object>();

            foreach (var item in value)
            {
                result.Add(orderValue(item));
            }
            return result;
        }

        /// Takes a generic [value] and returns its sorted representation.
        /// * If it is a map, [sort] is called.
        /// * If it is a list, [orderList] is called.
        /// * Otherwise, the same value is returned.
        private static Object orderValue(Object value)
        {
            // if (value is Dictionary)
            // This should be equivalent in C#
            if (value != null)
            {
                Type t = value.GetType();
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return sort(value as Dictionary<String, Object>);
                }
                else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                {
                    List<object> list = (value as IEnumerable<object>).Cast<object>().ToList();
                    return orderList(list);
                }
            }
            return value;
        }

        #endregion

    }
}
