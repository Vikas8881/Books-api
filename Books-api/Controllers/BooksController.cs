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
{/// <summary>
/// Interact with the Books Table
/// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, ILoggerService loggerService, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = loggerService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call");
                var books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(books);
                _logger.LogInfo($"{location}: Successfull");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}:-{e.Message}-{e.InnerException}");
            }
        }

        [HttpGet("{ID:int}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int ID)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call For Get Book ID: {ID}");
                var book = await _bookRepository.FindByID(ID);
                if (book == null)
                {
                    _logger.LogWarn($"{location}:Failed to retrive Book By ID: {ID}");
                    return NotFound();
                }
                var response = _mapper.Map<BookDTO>(book);
                _logger.LogInfo($"{location}: Successfully got Record with ID: {ID}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}:-{e.Message}-{e.InnerException}");
            }
        }
        /// <summary>
        /// Creates a New Book
        /// </summary>
        /// <param name="BookDto"></param>
        /// <returns>Book Object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDto BookDto)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}:Create Attempted");
                if (BookDto == null)
                {
                    _logger.LogWarn($"{location}:Empty Request was submitted");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(BookDto);
                var isSuccess = await _bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}:Creation Failed");
                }
                _logger.LogInfo($"{location}: Creation was successful");
                _logger.LogInfo($"{location}: {book}");

                return Created("Create", new { book });
            }
            catch (Exception)
            {

                return InternalError($"{location}:Creation Failed");
            }
        }
        [HttpPut("{ID}")]
        public async Task<IActionResult>Update(int ID, [FromBody] BookUpdateDTO bookUpdateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Book Update Attempted with following ID:{ID}");
                if(ID<1 || bookUpdateDTO==null|| ID!=bookUpdateDTO.ID)
                {
                    _logger.LogWarn($"{location}:Update failed with bad data-ID:{ID}");
                    return BadRequest();
                }
                var isExists = await _bookRepository.isExits(ID);
                if(!isExists)
                {
                    _logger.LogWarn($"{location}:Failed to retrive record with ID:{ID}");
                    return NotFound();
                }
                if(!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}:Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookUpdateDTO);
                var isSuccess = await _bookRepository.Update(book);
                if(!isSuccess)
                {
                    return InternalError($"{location}: Update Failed");
                }
                _logger.LogInfo($"{location}: Record with ID:{ID} successfully Updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}:{e.Message}-{e.InnerException}");
            }
        }
    /// <summary>
    /// Delete Book
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
        
        [HttpDelete("{ID}")]
     [ProducesResponseType(StatusCodes.Status204NoContent)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status404NotFound)]
     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>Delete(int ID)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Delete Attempted on Record with ID:{ID}");
                if(ID<1)
                {
                    _logger.LogWarn($"{location}:Delete Failed with bad Data-ID:{ID}");
                    return BadRequest();
                }
                var isExits = await _bookRepository.isExits(ID);
                if(!isExits)
                {
                    _logger.LogInfo($"Book with ID:{ID} was not found");
                    return NotFound();
                }
                var book = await _bookRepository.FindByID(ID);
                var isSucess = await _bookRepository.Delete(book);
                if(!isSucess)
                {
                    return InternalError($"{location}:Delete failed for record ID:{ID}");
                }
                _logger.LogInfo($"{location}: Deleted Successfully with ID:{ID}");

                return NoContent();
            }
            catch (Exception e)
            {

                return InternalError($"{e.Message}-{e.InnerException}");
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
