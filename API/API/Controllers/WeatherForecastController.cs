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

    // 1. ���������� �������� ��� ���� ������� � �����������
    // 2. ����� ��� ������ ������ ������������ �� ���������� �������
    [HttpGet("{index}")]
    public IActionResult GetItemByIndex(int index)
    {
        // �������� �� ������������� ������
        if (index < 0)
        {
            return BadRequest("������ �� ����� ���� �������������");
        }

        // �������� �� ����� �� ������� ������
        if (index >= _items.Count)
        {
            return NotFound($"������� � �������� {index} �� ������");
        }

        return Ok(_items[index]);
    }

    // 3. ����� ��� ��������� ���������� ������� �� ������������ �����
    [HttpGet("count/{name}")]
    public IActionResult GetCountByName(string name)
    {
        // �������� �� null ��� ������ ���
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("��� �� ����� ���� ������");
        }

        var count = _items.Count(item => item.Equals(name, StringComparison.OrdinalIgnoreCase));
        return Ok(count);
    }

    // 4. ����� ��������� ���� ������� � �������������� ���������� ����������
    [HttpGet]
    public IActionResult GetAll(int? sortStrategy = null)
    {
        IEnumerable<string> result = _items;

        // ��������� ��������� ����������
        if (sortStrategy.HasValue)
        {
            switch (sortStrategy.Value)
            {
                case 0:
                    // ��������� ������ ��� ����
                    break;
                case 1:
                    // ���������� �� �����������
                    result = _items.OrderBy(item => item);
                    break;
                case -1:
                    // ���������� �� ��������
                    result = _items.OrderByDescending(item => item);
                    break;
                default:
                    // ������������ �������� ���������
                    return BadRequest("������������ �������� ��������� sortStrategy");
            }
        }

        return Ok(result.ToList());
    }

    // ������ ������ ����������� � ����������...

    [HttpPost]
    public IActionResult AddItem([FromBody] string item)
    {
        // �������� �� null ��� ������ ������
        if (string.IsNullOrWhiteSpace(item))
        {
            return BadRequest("������� �� ����� ���� ������");
        }

        _items.Add(item);
        return Ok();
    }

    [HttpPut("{index}")]
    public IActionResult UpdateItem(int index, [FromBody] string newItem)
    {
        // �������� �� ������������� ������
        if (index < 0)
        {
            return BadRequest("������ �� ����� ���� �������������");
        }

        // �������� �� ����� �� ������� ������
        if (index >= _items.Count)
        {
            return NotFound($"������� � �������� {index} �� ������");
        }

        // �������� �� null ��� ������ ������
        if (string.IsNullOrWhiteSpace(newItem))
        {
            return BadRequest("������� �� ����� ���� ������");
        }

        _items[index] = newItem;
        return Ok();
    }

    [HttpDelete("{index}")]
    public IActionResult DeleteItem(int index)
    {
        // �������� �� ������������� ������
        if (index < 0)
        {
            return BadRequest("������ �� ����� ���� �������������");
        }

        // �������� �� ����� �� ������� ������
        if (index >= _items.Count)
        {
            return NotFound($"������� � �������� {index} �� ������");
        }

        _items.RemoveAt(index);
        return Ok();
    }
}
