
using Microsoft.AspNetCore.Mvc;
using MiniShop.Application.DTOs;
using MiniShop.Application.Interfaces;

namespace MiniShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id,CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request,CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Название товара обязательно.");

        if (request.Price <= 0)
            return BadRequest("Цена должна быть больше нуля.");

        if (request.ProductType != "Physical" &&request.ProductType != "Digital")
            return BadRequest("Тип товара должен быть Physical или Digital.");

        if (request.ProductType == "Physical")
        {
            if (request.StockQuantity < 0)
                return BadRequest("Остаток не может быть отрицательным.");

            if (request.ShippingCost < 0)
                return BadRequest("Стоимость доставки не может быть отрицательной.");
        }

        if (request.ProductType == "Digital" &&request.LicenseDurationDays <= 0)
            return BadRequest("Срок лицензии должен быть больше нуля.");

        var product = await _productService.CreateAsync(request.Name,request.Price,request.ProductType,request.StockQuantity,request.ShippingCost,request.LicenseDurationDays,cancellationToken);

        return CreatedAtAction(nameof(GetById),new { id = product.Id },product);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> Update(int id,[FromBody] UpdateProductRequest request,CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("ID товара должен быть положительным.");

        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Название товара обязательно.");

        if (request.Price <= 0)
            return BadRequest("Цена должна быть больше нуля.");

        var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);

        if (existingProduct is null)
            return NotFound();

        if (existingProduct.ProductType == "Physical")
        {
            if (request.StockQuantity < 0)
                return BadRequest("Остаток не может быть отрицательным.");

            if (request.ShippingCost < 0)
                return BadRequest("Стоимость доставки не может быть отрицательной.");
        }
        else if (existingProduct.ProductType == "Digital")
        {
            if (request.LicenseDurationDays <= 0)
                return BadRequest("Срок лицензии должен быть больше нуля.");
        }


        var product = await _productService.UpdateAsync(id,request.Name,request.Price,request.StockQuantity,request.ShippingCost,request.LicenseDurationDays,cancellationToken);

        if (product is null)
            return NotFound();

        return Ok(product);
    }



    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(
            id, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody] PurchaseRequest request,CancellationToken cancellationToken)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            return BadRequest("Корзина пуста.");
        }

        var success = await _productService.PurchaseAsync(request.Items,cancellationToken);

        if (!success)
        {
            return BadRequest("Не удалось выполнить покупку. Проверь товары в корзине.");
        }

        return Ok(new
        {
            message = "Покупка успешно выполнена."
        });
    }
}


public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ProductType { get; set; } = "Physical";

    public int StockQuantity { get; set; }

    public decimal ShippingCost { get; set; } = 200m;

    public int LicenseDurationDays { get; set; }
}

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public decimal ShippingCost { get; set; }

    public int LicenseDurationDays { get; set; }
}
