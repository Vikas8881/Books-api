using AutoMapper;
using Books_api.Contracts;
using Books_api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books_api.Controllers
{
    /// <summary>
    /// Endpoint Used to Interact with the authors in the Book Store's Database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService loggerService,IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = loggerService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Author's List
        /// </summary>
        /// <returns>List of Author's</returns>
        [HttpGet]
        public async Task<IActionResult>GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attempted Get All Authors");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("Successfully got all Authors");
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}-{e.InnerException}");
                return StatusCode(500,"Something went wrong.");
            }
           
        }

    }
}
