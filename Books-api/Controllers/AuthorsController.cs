using AutoMapper;
using Books_api.Contracts;
using Books_api.Data;
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

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService loggerService, IMapper mapper)
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
        public async Task<IActionResult> GetAuthors()
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
                return StatusCode(500, "Something went wrong.");
            }

        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetAuthor(int ID)
        {
            try
            {
                _logger.LogInfo("Attempted Get Authors with ID: {ID}");
                var author = await _authorRepository.FindByID(ID);
                if (author == null)
                {
                    _logger.LogWarn("Author Not Found with ID:{ID}");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo("Successfully Get Authors with ID: {ID}");
                return Ok(response);
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message}-{e.InnerException}");
            }
        }/// <summary>
         /// Creates an Author
         /// </summary>
         /// <param name="authorDTO"></param>
         /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                _logger.LogInfo($"Author Submission Attempted");
                if (authorDTO == null)
                {
                    _logger.LogWarn($"Empty Request was Submited");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Details was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Create(author);
                if (!isSuccess)
                {
                    _logger.LogWarn($"Author Creation Failed");
                    return InternalError($"Author Creation Failed");
                }
                _logger.LogInfo("Author Created Successfully");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message}-{e.InnerException}");
            }
        }
        /// <summary>
        /// Update an Author
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="authorUpdate"></param>
        /// <returns></returns>
        [HttpPut("{ID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int ID, [FromBody] AuthorUpdateDTO authorUpdate)
        {
            try
            {
                _logger.LogInfo("Author Update Attempted-ID:{ID}");
                if (ID < 1 || authorUpdate == null || ID != authorUpdate.ID)
                {
                    _logger.LogInfo("Author Update Failed with bad Data");
                    return BadRequest();
                }
                var isExits = await _authorRepository.isExits(ID);
                if (!isExits)
                {
                    _logger.LogInfo($"Author with ID:{ID} was not found");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("Author Data was incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorUpdate);
                var isSuccess = await _authorRepository.Update(author);
                if (!isSuccess)
                {
                    return InternalError($"Update Opration Failed");
                }
                return NoContent();
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message}-{e.InnerException}");
            }
        }
     /// <summary>
     /// Delete the Author
     /// </summary>
     /// <param name="ID"></param>
     /// <returns></returns>
       [HttpDelete("{ID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int ID)
        {
            try
            {
                _logger.LogInfo($"Author with ID:{ID} Delete Attempted");
                if(ID<1)
                {
                    _logger.LogInfo($"Author Delete Failed with bad Data");
                    return BadRequest();
                }
                var isExits = await _authorRepository.isExits(ID);
                if (!isExits)
                {
                    _logger.LogInfo($"Author with ID:{ID} was not found");
                    return NotFound();
                }
                var author = await _authorRepository.FindByID(ID);
                var isSuccess = await _authorRepository.Delete(author);
                if(!isSuccess)
                {
                    return InternalError($"Author Delete Failed");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message}-{e.InnerException}");
            }
    }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong.");
        }
    }
}
