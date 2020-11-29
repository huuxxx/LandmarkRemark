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

        [HttpGet("GetMyNotes/{userID}")]
        public async Task<IActionResult> GetMynotes(int userID)
        {
            // Return all notes of current user

            return Ok();
        }

        [HttpGet("GetOthersNotes/{userID}")]
        public async Task<IActionResult> GetOthersnotes(int userID)
        {
            // Return all notes of OTHER users

            return Ok();
        }

        [HttpGet("SearchNotes/{userName, noteText}")]
        public async Task<IActionResult> SearchNotes(string userName, string noteText)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(noteText))
            {
                return BadRequest();
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(noteText))
            {
                // Query using both params
                return Ok();
            }

            if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(noteText))
            {
                // Query using User Name only
                return Ok();
            }

            if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(noteText))
            {
                // Query using Note Text only
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("SaveNote/{userID, note, coordinateX, coordinateY}")]
        public async Task<IActionResult> SaveNote(int userId, string note, decimal coordinateX, decimal coordinateY)
        {
            try
            {
                string queryString = string.Format("INSERT INTO [TableName] (userID, note, coordinateX, coordinateY) VALUES ('{0}', '{1}', '{2}', '{3}')", userId, note, coordinateX, coordinateY);

                string connString = ConfigurationExtensions.GetConnectionString(configuration, "DB_ConnectionString");

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                }

                return Ok();
            }
            catch (Exception)
            {
                // Log failure
                return BadRequest();
            }
        }

    }
}
