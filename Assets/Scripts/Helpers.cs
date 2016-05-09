public enum Border {
	Top, Bottom, Left, Right
};

public enum Direction {
	None, Up, Down, Left, Right
};

public struct TileResourcesStruct {
	public int wood, coal, iron, copper, titan, silver, crudeoil, food;
};

public class TILES {
	public const int FLOOR 			= 0;
	public const int WALL			= 1;
	public const int WINDOW			= 2;
	public const int GRASS	 		= 3;
	public const int TREE		 	= 4;
	public const int WATER		 	= 5;
	public const int COUNT		 	= 6;
}