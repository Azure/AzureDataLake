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
            // Handle the first page
            var t_array = page_items_to_array(page);
            yield return t_array;

            // Handle the remaining pages
            while (!string.IsNullOrEmpty(page.NextPageLink))
            {
                page = f_get_next_page(page);

                var t_array_next = page_items_to_array(page);
                yield return t_array_next;

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