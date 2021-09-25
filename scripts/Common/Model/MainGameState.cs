using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MainGameState
{
    public Dictionary<string, object> AllData { get; private set; }

    private MainGameState()
    {
        AllData = new Dictionary<string, object>();
    }

    public void AddData(Dictionary<string, object> newData)
    {
        foreach(var key in newData.Keys)
        {
            AllData[key] = newData[key];
        }
    }

    public void Clear()
    {
        AllData = new Dictionary<string, object>();
    }

    public static MainGameState Instance
    {
        get
        {
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = new MainGameState();
                }
                return _instance;
            }
        }
    }

    private static MainGameState _instance = null;
    private static readonly object _padlock = new object();
}
