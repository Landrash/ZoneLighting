using System.Drawing;

namespace Graphics
{
	public interface IPixel
	{
		Color Color { get; set; }
	    int Index { get; set; }
    }
}