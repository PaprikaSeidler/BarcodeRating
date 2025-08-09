using Microsoft.AspNetCore.Mvc;
using BarcodeRatingLib;
using BarcodeRatingLib.Models;
using restApi.Records;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly RatingRepository? _ratingRepository;
        public RatingsController(RatingRepository? ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // GET: api/<RatingsController>
        [HttpGet]
        public ActionResult<IEnumerable<Rating>> GetAll()
        {
            IEnumerable<Rating> ratings = _ratingRepository.GetAll();
            if (ratings.Any())
            {
                return Ok(ratings);
            }
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET api/<RatingsController>/5
        [HttpGet("byid/{id}")]
        public ActionResult<Rating> GetbyId(int id)
        {
            Rating? rating = _ratingRepository.GetById(id);
            if (rating != null)
            {
                return Ok(rating);
            }
            return NotFound();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET api/<RatingsController>/123456789000
        [HttpGet("bybarcode/{ProductBarcode}")]
        public ActionResult<Rating> GetByBarcode(long ProductBarcode)
        {
            IEnumerable<Rating> ratingsByBarcode = _ratingRepository.GetByProductBarcode(ProductBarcode);
            if (ratingsByBarcode != null)
            {
                return Ok(ratingsByBarcode);
            }
            return NotFound();
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // POST api/<RatingsController>
        [HttpPost]
        public ActionResult<Rating> Post([FromBody] RatingRecord newRatingRecord)
        {
            try
            {
                Rating converted = RatingRecordHelper.ConvertRatingRecord(newRatingRecord);
                Rating addedRating = _ratingRepository.Add(converted);
                return Created($"/api/ratings/{addedRating.Id}", addedRating);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // PUT api/<RatingsController>/5
        [HttpPut("{id}")]
        public ActionResult<Rating> Put(int id, [FromBody] RatingRecord updatedRatingRecord)
        {
            try
            {
                Rating converted = RatingRecordHelper.ConvertRatingRecord(updatedRatingRecord);
                Rating updated = _ratingRepository.Update(converted);
                if (updated != null)
                {
                    return Ok(updated);
                }
                return NotFound();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // DELETE api/<RatingsController>/5
        [HttpDelete("{id}")]
        public ActionResult<Rating> Delete(int id)
        {
            Rating? deletedRating = _ratingRepository.Delete(id);
            if (deletedRating != null)
            {
                return Ok(deletedRating);
            }
            return NotFound();
        }
    }
}
