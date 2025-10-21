using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<string> _items = new List<string>
    {
        "Apple", "Banana", "Orange", "Grape", "Mango"
    };

    // 1. ƒобавление проверок дл€ всех методов с параметрами
    // 2. ћетод дл€ вывода одного наименовани€ по указанному индексу
    [HttpGet("{index}")]
    public IActionResult GetItemByIndex(int index)
    {
        // ѕроверка на отрицательный индекс
        if (index < 0)
        {
            return BadRequest("»ндекс не может быть отрицательным");
        }

        // ѕроверка на выход за границы списка
        if (index >= _items.Count)
        {
            return NotFound($"Ёлемент с индексом {index} не найден");
        }

        return Ok(_items[index]);
    }

    // 3. ћетод дл€ получени€ количества записей по указываемому имени
    [HttpGet("count/{name}")]
    public IActionResult GetCountByName(string name)
    {
        // ѕроверка на null или пустое им€
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("»м€ не может быть пустым");
        }

        var count = _items.Count(item => item.Equals(name, StringComparison.OrdinalIgnoreCase));
        return Ok(count);
    }

    // 4. ћетод получени€ всех записей с необ€зательным параметром сортировки
    [HttpGet]
    public IActionResult GetAll(int? sortStrategy = null)
    {
        IEnumerable<string> result = _items;

        // ќбработка параметра сортировки
        if (sortStrategy.HasValue)
        {
            switch (sortStrategy.Value)
            {
                case 0:
                    // ќставл€ем список как есть
                    break;
                case 1:
                    // —ортировка по возрастанию
                    result = _items.OrderBy(item => item);
                    break;
                case -1:
                    // —ортировка по убыванию
                    result = _items.OrderByDescending(item => item);
                    break;
                default:
                    // Ќекорректное значение параметра
                    return BadRequest("Ќекорректное значение параметра sortStrategy");
            }
        }

        return Ok(result.ToList());
    }

    // ƒругие методы контроллера с проверками...

    [HttpPost]
    public IActionResult AddItem([FromBody] string item)
    {
        // ѕроверка на null или пустую строку
        if (string.IsNullOrWhiteSpace(item))
        {
            return BadRequest("Ёлемент не может быть пустым");
        }

        _items.Add(item);
        return Ok();
    }

    [HttpPut("{index}")]
    public IActionResult UpdateItem(int index, [FromBody] string newItem)
    {
        // ѕроверка на отрицательный индекс
        if (index < 0)
        {
            return BadRequest("»ндекс не может быть отрицательным");
        }

        // ѕроверка на выход за границы списка
        if (index >= _items.Count)
        {
            return NotFound($"Ёлемент с индексом {index} не найден");
        }

        // ѕроверка на null или пустую строку
        if (string.IsNullOrWhiteSpace(newItem))
        {
            return BadRequest("Ёлемент не может быть пустым");
        }

        _items[index] = newItem;
        return Ok();
    }

    [HttpDelete("{index}")]
    public IActionResult DeleteItem(int index)
    {
        // ѕроверка на отрицательный индекс
        if (index < 0)
        {
            return BadRequest("»ндекс не может быть отрицательным");
        }

        // ѕроверка на выход за границы списка
        if (index >= _items.Count)
        {
            return NotFound($"Ёлемент с индексом {index} не найден");
        }

        _items.RemoveAt(index);
        return Ok();
    }
}
