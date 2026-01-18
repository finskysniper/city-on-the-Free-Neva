public class HexCell
{
    public int q; // axial
    public int r;

    public bool walkable = true;
    public int moveCost = 1;

    public HexCell(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
}
