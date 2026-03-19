using System;

namespace BackpackApp.Models
{
    /// <summary>
    /// Класс предмета для задачи о рюкзаке
    /// </summary>
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
    }
}