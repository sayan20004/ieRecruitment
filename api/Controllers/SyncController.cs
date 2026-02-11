using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using api.Model; // <--- This must match the namespace in SyncModel.cs

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SyncController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("save-offline-data")]
        public async Task<IActionResult> SaveOfflineData([FromBody] SyncModel request)
        {
            // Validate Request
            if (request == null || string.IsNullOrEmpty(request.CandidateId) || string.IsNullOrEmpty(request.LocalStorageData))
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            // Get Connection String securely
            string connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Ensure your SQL Stored Procedure 'sp_SyncLocalDataToDb' exists in the DB
                    using (SqlCommand cmd = new SqlCommand("sp_SyncLocalDataToDb", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CandidateId", request.CandidateId);
                        cmd.Parameters.AddWithValue("@JsonData", request.LocalStorageData);

                        await con.OpenAsync();
                        var result = await cmd.ExecuteScalarAsync();
                        
                        return Ok(new { success = true });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}