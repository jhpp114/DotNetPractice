using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Utility
{
    public static class StaticDetail
    {
        public const string DefaultImage = "default_food.png";
        public const string MANAGER_USER = "Manager";
        public const string KITCHEN_USER = "Kitchen";
        public const string FRONTDESK_USER = "FrontDesk";
        public const string CUSTOMER_END_USER = "Customer";

		public static string ConvertToRawHtml(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}

	}

}
