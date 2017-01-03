using System.Collections.Generic;

public class MDUnit {
 
    public string title = "Ivan";
    public string type;
    public GDSprite texture;
    public bool friendly;
    public int ap, apMax;
    
    public MDUnit(string type) {
        this.type = type;
        this.texture = ResManager.shared.TextureForUnit(type);
    }
}