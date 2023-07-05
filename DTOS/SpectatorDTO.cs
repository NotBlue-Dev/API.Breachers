using System.Collections.Generic;

namespace BreachersAPI.DTOS;

public class SpectatorDTO
{
    public List<float> Position { get; set; }
    public List<float> Forward { get; set; }
    public List<float> Right { get; set; }
    public List<float> Up { get; set; }
    public List<float> Rotation { get; set; }
    
    public string SpectatingPlayer { get; set; }
}