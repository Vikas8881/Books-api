using AutoMapper;
using Books_api.Contracts;
using Books_api.Data;
using Books_api.DTOs;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService loggerService, 
            IMapper mapper)
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
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}: Attempted Get All Authors");
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthor(int ID)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Get Authors with ID: {ID}");
                var author = await _authorRepository.FindByID(ID);
                if (author == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {ID}");
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
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Create Attempted");
                if (authorDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty Request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Create(author);
                if (!isSuccess)
                {
                    _logger.LogWarn($"Author Creation Failed");
                    return InternalError($"{location}: Creation failed");
                }
                _logger.LogInfo($"{location}: Creation was successful");
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
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int ID, [FromBody] AuthorUpdateDTO authorUpdate)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Update Attempted on record with id: {ID} ");
                if (ID < 1 || authorUpdate == null || ID != authorUpdate.ID)
                {
                    _logger.LogInfo("Author Update Failed with bad Data");
                    return BadRequest();
                }
                var isExits = await _authorRepository.isExits(ID);
                if (!isExits)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {ID}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorUpdate);
                var isSuccess = await _authorRepository.Update(author);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Update failed for record with id: {ID}");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Delete the Author
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        
        [HttpDelete("{ID}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int ID)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Delete Attempted on record with id: {ID} ");

                if (ID<1)
                {
 _logger.LogWarn($"{location}: Delete failed with bad data - id: {ID}");

                    return BadRequest();
                }
                var isExits = await _authorRepository.isExits(ID);
                if (!isExits)
                {
                    _logger.LogInfo($"{location}:Author with ID:{ID} was not found");
                    return NotFound();
                }
                var author = await _authorRepository.FindByID(ID);
                var isSuccess = await _authorRepository.Delete(author);
                if(!isSuccess)
                {
                    return InternalError($"{location}:Author Delete Failed");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message}-{e.InnerException}");
            }
    }
        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller}-{action}";
        }
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong.");
        }
    }
}
