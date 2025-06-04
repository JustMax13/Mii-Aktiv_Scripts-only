using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Filters.FilteringValues.Category
{
    public class Categories
    {
        private static readonly string _pathToPicture = "Sprites/New/2 - Category/Images/";
        private static readonly Dictionary<int, CategoryValue> _allCategoriesName = new Dictionary<int, CategoryValue>()
        {
            // ВАЖЛИВО! При змінні категорій ( ID, назви ), події будуть посилатися
            // на не коректні по своїй суті категорії, а трекінг системи будуть вказувати 
            // не актуальні дані. Для потрібної послідовності спавну можна перемішувати тут порядок 
            // об'єктів
         // { ID, CategoryValue }
            { 0, new CategoryValue("Волонтерство", _pathToPicture + "Volunteering") },
            { 1, new CategoryValue("Курси та вебінари",  _pathToPicture + "Cources") },
            { 2, new CategoryValue("Відпочинок та розваги", _pathToPicture + "Rest") },
            { 3, new CategoryValue("Здоров'я та спорт",  _pathToPicture + "Health and sport") },
            { 4, new CategoryValue("Сезоні", _pathToPicture + "Seasonal") },
            { 5, new CategoryValue("Змагання", _pathToPicture + "Competition") },
            { 6, new CategoryValue("Творчість", _pathToPicture + "Createve") },
            { 7, new CategoryValue("Для студентів", _pathToPicture + "For students") },
            { 8, new CategoryValue("Майстер класи", _pathToPicture + "Master class") },
            { 9, new CategoryValue("Події у КПІ", _pathToPicture + "KPI") },
        };

        public static Dictionary<int, CategoryValue> AllCategories => _allCategoriesName;
    }
}