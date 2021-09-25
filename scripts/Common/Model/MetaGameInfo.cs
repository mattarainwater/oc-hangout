using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MetaGameInfo
{
    public List<GameSave> GameSaves { get; set; }

    public MetaGameInfo()
    {
        GameSaves = new List<GameSave>();
    }
}
