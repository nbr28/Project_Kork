using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Snippet {
    public int id;
    public string title;
    public string contents;
    public List<string> tags;
    public float x, y, z;
    public string titleBarColor;
}
