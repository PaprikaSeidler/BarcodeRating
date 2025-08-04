using Microsoft.AspNetCore.Mvc;
using BarcodeRatingLib;
using BarcodeRatingLib.Models;
using restApi.Records;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository? _productRepository;

        public ProductsController(ProductRepository? productRepository)
        {
            _productRepository = productRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            if (products.Any())
            {
                return Ok(products);
            }
            else
            {
                return NoContent();
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET api/<ProductsController>/12334556
        [HttpGet("{barcode}")]
        public ActionResult<Product> Get(int barcode)
        {
            Product? product = _productRepository.GetByBarcode(barcode);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();    
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult Post([FromBody] ProductRecord newProductRecord)
        {
            try
            {
                Product converted = ProductRecordHelper.ConvertProductRecord(newProductRecord);
                Product addedProduct = _productRepository.Add(converted);
                return Created("/" + addedProduct.Barcode, addedProduct);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Can't contain null" + ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest("Barcode out of range" + ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Invalid argument..." + ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // DELETE api/<ProductsController>/5
        [HttpDelete("{barcode}")]
        public ActionResult<Product> Delete(int barcode)
        {
            Product? deleted = _productRepository.Delete(barcode);
            if (deleted != null)
            {
                return Ok(deleted);
            }
            return NotFound();
        }
    }
}
