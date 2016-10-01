using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest.Azure;
using ADL=Microsoft.Azure.Management.DataLake;

namespace AzureDataLake
{
    public class RESTUtil
    {
        public static IEnumerable<T[]> EnumPages<T>(IPage<T> page, System.Func<IPage<T>, IPage<T>> f_get_next_page)
        {
            var t_array = page_items_to_array(page);
            yield return t_array;

            // While there are additional pages left fetch them
            while (!string.IsNullOrEmpty(page.NextPageLink))
            {
                var t_array_2 = page_items_to_array(page);

                yield return t_array_2;
                page = f_get_next_page(page);
            }
        }

        public static T[] page_items_to_array<T>(Microsoft.Rest.Azure.IPage<T> page)
        {
            int num_items_in_page = page.Count();
            var items = new T[num_items_in_page];

            int i = 0;
            foreach (var item in page)
            {
                items[i] = item;
                i++;
            }
            return items;
        }

    }
}