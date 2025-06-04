using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Filters.FilteringValues.Category
{
    public struct CategoryValue
    {
        public string Name { get; set; }
        /// <summary>
        /// Патч до фото записується з врахуванням, що воно розміщено
        /// у папці Resources
        /// </summary>
        public string PathToIcon { get; set; }
        public CategoryValue(string name)
        {
            Name = name;
            PathToIcon = "";
        }
        /// <summary>
        /// <param name="pathToIcon">Патч до фото записується з врахуванням, що воно розміщенно
        /// у папці Resources, а також не вказується формат. </param>
        /// </summary>
        public CategoryValue(string name, string pathToIcon)
        {
            Name = name;
            PathToIcon = pathToIcon;
        }
    }
}

