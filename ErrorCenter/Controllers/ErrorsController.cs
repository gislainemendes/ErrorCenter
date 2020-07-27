using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Central_De_Erros.Models;
using Central_De_Erros.Repository;
using Central_De_Erros.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Central_De_Erros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ErrorsController : ControllerBase
    {
        private readonly IErrorRepository repoError;
        private readonly IFrequencyRepository repoFrequency;
        private readonly IMapper _mapper;
        private List<string> levels = new List<string>() { "DEBUG", "INFO", "WARNING", "ERROR", "FATAL", "OFF", "TRACE" };
        private List<string> environments = new List<string>() { "DEVELOPMENT", "PRODUCTION", "HOMOLOGATION" };

        public ErrorsController(IErrorRepository repoError, IFrequencyRepository repoFrequency, IMapper mapper)
        {
            this.repoFrequency = repoFrequency;
            this.repoError = repoError;
            _mapper = mapper;
        }

        /// <summary> Return a list of registered errors by level </summary>
        /// <param name="level"></param>
        /// <response code="200">If an error with this level exists</response>
        /// <response code="204">This search did not find results</response>
        /// <response code="400">The level doesn't exist</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Level")]
        public ActionResult<IEnumerable<ErrorViewModel>> GetErrorByLevel(string level)
        {
            if (!levels.Contains(level))
            {
                return BadRequest("Level doesn't exist");
            }

            IEnumerable<ErrorViewModel> errors = repoError.FindByLevel(level)
                                                             .Select(error => _mapper.Map<ErrorViewModel>(error))
                                                             .ToList();
            if (errors.Count() == 0)
            {
                return NoContent();
            }

            return Ok(errors);
        }

        /// <summary> Return a list of registered errors by log description </summary>
        /// <param name="description"></param>
        /// <response code="200">If an error with this log description exists</response>
        /// <response code="204">This search did not find results</response>
        /// <response code="400">The log description doesn't exist</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Description")]
        public ActionResult<IEnumerable<ErrorViewModel>> GetErrorByDescription(string description)
        {
            if (description == null)
            {
                return BadRequest("Log description doesn't exist");
            }

            IEnumerable<ErrorViewModel> errors = repoError.FindByLogDescription(description)
                                                            .Select(error => _mapper.Map<ErrorViewModel>(error))
                                                            .ToList();

            if (errors.Count() == 0)
            {
                return NoContent();
            }

            return Ok(errors);
        }

        /// <summary> Return a list of registered errors by Ip origin </summary>
        /// <param name="IpOrigin"></param>
        /// <response code="200">If an error with this Ip origin exists</response>
        /// <response code="204">This search did not find results</response>
        /// <response code="400">The Ip origin doesn't exist</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Ip")]
        public ActionResult<IEnumerable<ErrorViewModel>> GetErrorByIpOrigin(string IpOrigin)
        {
            if (IpOrigin == null)
            {
                return BadRequest("Ip origin doesn't exist");
            }

            IEnumerable<ErrorViewModel> errors = repoError.FindByIpOrigin(IpOrigin)
                                                            .Select(error => _mapper.Map<ErrorViewModel>(error))
                                                            .ToList();

            if (errors.Count() == 0)
            {
                return NoContent();
            }

            return Ok(errors);
        }

        /// <summary> Return a list of registered errors by environment and ordered by level </summary>
        /// <param name="environment"></param>
        /// <response code="200">If an error with this environment exists</response>
        /// <response code="204">This search did not find results</response>
        /// <response code="400">The environment doesn't exist</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("EnvironmentOrderedByLevel")]
        public ActionResult<IEnumerable<ErrorViewModel>> EnvironmentOrderedByLevel(string environment)
        {
            if (!environments.Contains(environment))
            {
                return BadRequest("Environment doesn't exist");
            }

            IEnumerable<ErrorViewModel> errors = repoError.FindByEnvironmentAndOrderByLevel(environment)
                                                            .Select(error => _mapper.Map<ErrorViewModel>(error))
                                                            .ToList();

            if (errors.Count() == 0)
            {
                return NoContent();
            }

            return Ok(errors);
        }

        /// <summary> Return a list of registered errors select by environment. This list is ordered by number of repeated events which are defined by a description </summary>
        /// <param name="environment"></param>
        /// <response code="200">If an error with this environment exists</response>
        /// <response code="204">This search did not find results</response>
        /// <response code="400">The environment doesn't exist</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("EnvironmentOrderedByFrequency")]
        public ActionResult<IEnumerable<ErrorViewModel>> EnvironmentOrderedByFrequency(string environment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!environments.Contains(environment))
            {
                return BadRequest("Environment doesn't exist");
            }

            IEnumerable<ErrorViewModel> errors = repoError.FindByEnvironmentAndOrderByFrequency(environment)
                                                            .Select(error => _mapper.Map<ErrorViewModel>(error))
                                                            .ToList();
            if (errors.Count() == 0)
            {
                return NoContent();
            }

            return Ok(errors);
        }


        /// <summary> Return a list of registered errors select by environment </summary>
        /// <param name="environment"></param>
        /// <response code="200">If an error with this environment exists</response>
        /// <response code="204">This search did not find results</response>
        /// <response code="400">The environment doesn't exist</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetAllByEnvironment")]
        public ActionResult<IEnumerable<ErrorViewModel>> GetAllByEnvironment(string environment)
        {
            if (!environments.Contains(environment))
            {
                return BadRequest("Environment doesn't exist");
            }

            IEnumerable<ErrorViewModel> errors = repoError.FindByEnvironment(environment)
                                                            .Select(error => _mapper.Map<ErrorViewModel>(error))
                                                            .ToList();

            if (errors.Count() == 0)
            {
                return NoContent();
            }

            return Ok(errors);
        }

        /// <summary> Delete an error by Id </summary>
        /// <param name="id"></param>
        /// <returns>The error was deleted</returns>
        /// <response code="200">The error was deleted</response>
        /// <response code="400">The error doesn't exist</response> 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteError([FromRoute] int id)
        {

            if (!ErrorExists(id))
            {
                return BadRequest("Error doesn't exist");
            }
            else
            {
                repoFrequency.SubtractFrequency(repoError.FindById(id).LogDescription);
                repoError.DeleteError(id);
                return Ok();
            }

        }

        /// <summary> Creates an error </summary>
        /// <returns>The error was created</returns>
        /// <response code="200">The error was created</response>
        /// <response code="400">The information is invalid</response> 
        [HttpPost("SaveError")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Error> SaveError([FromBody] ErrorDTO errorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Error error = _mapper.Map<Error>(errorDTO);

            if (!levels.Contains(error.Level))
            {
                return BadRequest("Level should be DEBUG, INFO, WARNING, ERROR, FATAL, OFF or TRACE");
            }
            else if (!environments.Contains(error.Environment))
            {
                return BadRequest("Environment should be DEVELOPMENT, PRODUCTION or HOMOLOGATION");
            }

            if (error.Id == 0)
            {
                error.frequency = repoFrequency.SumToFrequency(error.LogDescription);
            }

            repoError.SaveError(error);

            return Ok(error);
        }


        /// <summary> Return an error by Id </summary>
        /// <returns>The error was created</returns>
        /// <response code="200">An error with this Id was found</response>
        /// <response code="204">No content</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Error> FindError([FromRoute] int id)
        {

            var error = repoError.FindById(id);

            if (error == null)
            {
                return NoContent();
            }

            return Ok(error);
        }

        /// <summary>Archive an error by id</summary>
        /// <response code="200">The error was archived</response>
        /// <response code="204">No content</response>
        [HttpPut("ArchiveById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Boolean> ArchiveById(int id)
        {

            if (repoError.archiveErrorById(id))
            {
                return Ok();
            }
            return NoContent();
        }


        private bool ErrorExists(int id)
        {
            return repoError.FindById(id) != null;
        }
    }
}