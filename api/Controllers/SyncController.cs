using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ieRecruitment.Models;
using System.Data;
using System.Threading.Tasks;

namespace ieRecruitment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly string _connectionString;

        public SyncController(IConfiguration configuration)
        {
            // Ensure this matches your appsettings.json connection string name
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("save-offline-data")]
        public async Task<IActionResult> SaveOfflineData([FromBody] OfflineSyncRequest request)
        {
            if (string.IsNullOrEmpty(request.CandidateId) || string.IsNullOrEmpty(request.LocalStorageData))
            {
                return BadRequest("Invalid data provided.");
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SyncLocalDataToDb", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters matching the SP
                        cmd.Parameters.AddWithValue("@CandidateId", request.CandidateId);
                        cmd.Parameters.AddWithValue("@JsonData", request.LocalStorageData);

                        await con.OpenAsync();
                        
                        // Execute and read the simple status message from SP
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var status = reader.GetInt32(0); // 1 for success, 0 for error
                                var message = reader.GetString(1);

                                if (status == 1)
                                    return Ok(new { success = true, message = message, timestamp = System.DateTime.Now });
                                else
                                    return StatusCode(500, new { success = false, message = message });
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Server error: " + ex.Message });
            }

            return BadRequest("Unknown error occurred.");
        }
    }
}