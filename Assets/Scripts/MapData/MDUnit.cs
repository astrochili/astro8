using System.Collections.Generic;

public class MDUnit {
 
    public int type;
    public int texture;
    public bool friendly;
    public int ap, apMax;
    
    public MDUnit(int type, int texture = 0) {
        this.type = type;
        this.friendly = friendlyUnits.Contains(type);
        if (texture > 0) {
            this.texture = texture;
        } else {
            // Get Random Texture 
        }
    }
}