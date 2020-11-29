using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LandmarkRemark.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace LandmarkRemark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandmarkRemarkController : ControllerBase
    {
        private readonly IConfiguration configuration;

        // Constructor Dependency Injection
        public LandmarkRemarkController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Return all notes of current user
        /// </summary>
        /// <param name="userID">User ID the request was made from</param>
        /// <returns></returns>
        [HttpGet("GetMyNotes/{userID}")]
        public async Task<IActionResult> GetMynotes(int userID)
        {
            // Return all notes of current user

            return Ok();
        }

        /// <summary>
        /// Return all notes of OTHER users
        /// </summary>
        /// <param name="userID">User ID the request was made from</param>
        /// <returns></returns>
        [HttpGet("GetOthersNotes/{userID}")]
        public async Task<IActionResult> GetOthersnotes(int userID)
        {
            // Return all notes of OTHER users

            return Ok();
        }

        /// <summary>
        /// Query the database for notes by User Name and/or Note Text
        /// </summary>
        /// <param name="userName">User Name to be queried</param>
        /// <param name="noteText">User Name to be queried</param>
        /// <returns></returns>
        [HttpGet("SearchNotes/{userName, noteText}")]
        public async Task<IActionResult> SearchNotes(string userName, string noteText)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(noteText))
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(noteText))
                {
                    // Query using both params
                }

                if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(noteText))
                {
                    // Query using User Name only
                }

                if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(noteText))
                {
                    // Query using Note Text only
                }

                return Ok();
            }

            catch (Exception)
            {
                // Error Logging
                return BadRequest();
            } 
        }

        /// <summary>
        /// Save a new note to the database
        /// </summary>
        /// <param name="userId">User ID the request was made from</param>
        /// <param name="note">Text to be added</param>
        /// <param name="coordinateX">X GPS Coordinate for the note</param>
        /// <param name="coordinateY">Y GPS Coordinate for the note</param>
        /// <returns></returns>
        [HttpPost("SaveNote/{userID, note, coordinateX, coordinateY}")]
        public async Task<IActionResult> SaveNote(int userId, string note, decimal coordinateX, decimal coordinateY)
        {
            try
            {
                string queryString = string.Format("INSERT INTO [TableName] (userID, note, coordinateX, coordinateY) VALUES ('{0}', '{1}', '{2}', '{3}')", userId, note, coordinateX, coordinateY);

                string connectionString = ConfigurationExtensions.GetConnectionString(configuration, "DB_ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                }

                return Ok();
            }
            catch (Exception)
            {
                // Error Logging
                return BadRequest();
            }
        }
    }
}
