var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 1. ADD CORS POLICY ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5000", "http://localhost:7123") // Add your Frontend URL here
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// --------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- 2. USE CORS ---
app.UseCors("AllowFrontend");
// -------------------

app.UseAuthorization();
app.MapControllers();
app.Run();