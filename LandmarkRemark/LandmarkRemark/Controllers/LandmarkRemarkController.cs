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
        private readonly string connectionString;

        // Constructor Dependency Injection
        public LandmarkRemarkController(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = ConfigurationExtensions.GetConnectionString(configuration, "DB_ConnectionString");
        }

        /// <summary>
        /// Return all notes for current user OR all other users, depending on value of param 'selfNotes'
        /// </summary>
        /// <param name="userID">User ID the request was made from</param>
        /// <param name="selfNotes">If 'true' will return notes for the CURRENT user, if false it will return notes of all OTHER users</param>
        /// <returns>A list of SavedLocationNoteDTO</returns>
        [HttpGet("GetNotes/{userID}/{selfNotes}")]
        public async Task<ActionResult<List<SavedLocationNoteDTO>>> GetNotes(int userID, bool selfNotes)
        {
            try
            {
                string queryString;

                if (selfNotes)
                {
                    queryString = string.Format("SELECT CoordinateX, CoordinateY, Note FROM [Table] WHERE ID = {0}", userID);
                }
                else 
                {
                    queryString = string.Format("SELECT CoordinateX, CoordinateY, Note, UserName FROM [Table] WHERE ID != {0}", userID);
                }
                
                List<SavedLocationNoteDTO> savedLocationNoteDTO = new List<SavedLocationNoteDTO>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    // *** Automapping goes here! ***                    

                    reader.Close();
                }

                return savedLocationNoteDTO;
            }

            catch (Exception)
            {
                // Error logging
                return BadRequest();
            }
        }

        /// <summary>
        /// Query the database for notes by User Name and/or Note Text
        /// </summary>
        /// <param name="userName">User Name to be queried</param>
        /// <param name="noteTextSearch">Note text to be sarched for</param>
        /// <returns>SavedLocationNoteDTO</returns>
        [HttpGet("SearchNotes/{userName}/{noteText}")]
        public async Task<IActionResult> SearchNotes(string userName, string noteTextSearch)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(noteTextSearch))
                {
                    return BadRequest();
                }

                string queryString = "";

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(noteTextSearch))
                {
                    // Query both parameters
                    queryString = string.Format("SELECT CoordinateX, CoordinateY, Note, UserName FROM [Table] WHERE UserName = {0} AND Note = '%{1}%'", userName, noteTextSearch);
                }

                if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(noteTextSearch))
                {
                    // Query using User Name only
                    queryString = string.Format("SELECT CoordinateX, CoordinateY, Note, UserName FROM [Table] WHERE UserName = {0}", userName);
                }

                if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(noteTextSearch))
                {
                    // Query using Note Text only
                    queryString = string.Format("SELECT CoordinateX, CoordinateY, Note, UserName FROM [Table] WHERE Note = '%{0}%'", noteTextSearch);
                }

                SavedLocationNoteDTO savedLocationNoteDTO = new SavedLocationNoteDTO();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(queryString, connection);

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        savedLocationNoteDTO.UserName = reader["UserName"].ToString();
                        savedLocationNoteDTO.MessageNote = reader["Note"].ToString();
                        savedLocationNoteDTO.CoordinateX = (decimal)reader["CoordinateX"];
                        savedLocationNoteDTO.CoordinateY = (decimal)reader["CoordinateX"];
                    }

                    reader.Close();
                }

                return (IActionResult)savedLocationNoteDTO;
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
        /// <returns>ActionResult Ok</returns>
        [HttpPost("SaveNote/{userID}/{note}/{coordinateX}/{coordinateY}")]
        public IActionResult SaveNote(int userId, string note, decimal coordinateX, decimal coordinateY)
        {
            try
            {
                string queryString = string.Format("INSERT INTO [Table] (userID, note, coordinateX, coordinateY) VALUES ('{0}', '{1}', '{2}', '{3}')", userId, note, coordinateX, coordinateY);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    connection.Close();
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
