using System.Collections.Generic;

public class OBJECTS {
	public const int DOOR 			= 0;
	public const int BOX			= 1;
	public const int BED			= 2;
	public const int TERMINAL 		= 3;
	
    public const int count		 	= 4;
}

public class MDObject {
    
    static HashSet<int> solidTypes = new HashSet<int>() {OBJECTS.BED, OBJECTS.BOX, OBJECTS.TERMINAL};  

    public int type;
    public int texture;
    public bool solid;
    
    public MDObject(int type, int texture = 0) {
        this.type = type;
        solid = solidTypes.Contains(type);
        if (texture > 0) {
            this.texture = texture;
        } else {
            // Get Random Texture 
        }
    }

}