[System.Serializable]
public class ScreenResolution
{
    public int width;
    public int height;
    public int position;
    
    public ScreenResolution()
    {
        this.width = 1600;
        this.height = 900;
        this.position = 1;
    }

    public ScreenResolution(int width, int height, int position)
    {
        this.width = width;
        this.height = height;
        this.position = position;
    }
    public int getWidth() {
        return width;
    }
    
    public int getHeight() {
        return height;
    }

    public void setWidth(int width)
    {
        this.width = width;
    }
    
    public void setHeight(int height)
    {
        this.height = height;
    }
}