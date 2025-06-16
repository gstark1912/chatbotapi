using Microsoft.AspNetCore.Mvc;
using ChatbotAPI.Services;
using ChatbotAPI.Models;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        /// <summary>
        /// Gets all offers
        /// </summary>
        /// <returns>List of all offers</returns>
        [HttpGet]
        public async Task<ActionResult<List<Offer>>> GetAll()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return Ok(offers);
        }

        /// <summary>
        /// Gets only active offers
        /// </summary>
        /// <returns>List of active offers</returns>
        [HttpGet("active")]
        public async Task<ActionResult<List<Offer>>> GetActive()
        {
            var offers = await _offerService.GetActiveAsync();
            return Ok(offers);
        }
    }
}