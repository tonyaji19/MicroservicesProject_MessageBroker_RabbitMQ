using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Services;
using System.Text.Json;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMessageBrokerService _messageBrokerService;

        public ProductsController(IProductRepository productRepository, IMessageBrokerService messageBrokerService)
        {
            _productRepository = productRepository;
            _messageBrokerService = messageBrokerService;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_productRepository.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productRepository.GetById(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            _productRepository.Add(product);
            _messageBrokerService.PublishMessage(JsonSerializer.Serialize(product));
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();

            _productRepository.Update(product);
            _messageBrokerService.PublishMessage(JsonSerializer.Serialize(product));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.Delete(id);
            _messageBrokerService.PublishMessage($"Product with ID {id} deleted");
            return NoContent();
        }
    }
}
