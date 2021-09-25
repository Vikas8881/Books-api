﻿using Books_api.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books_api.Controllers
{/// <summary>
/// A Controller For Testing
/// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Get The Value
        /// </summary>
        /// <returns></returns>
        // GET: api/<DefaultController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Access Home Controller");
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<DefaultController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        // POST api/<DefaultController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/<DefaultController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<DefaultController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
