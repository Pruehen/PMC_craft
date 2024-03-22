struct Position
{
    public int x;//0~15
    public int y;//0~15

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Position()
    {
        x = 0;
        y = 0;
    }

    public void SetPosition(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    public static string PrintPosition(Position position)
    {
        return $"({(char)(position.y + 'A')}{position.x + 1})";
    }

    public static Position zero()//강렬해지는 플라멩코 음악
    {
        return new Position(0, 0);
    }
    public static int Distance(Position start, Position end)
    {
        int x = end.x - start.x;
        int y = end.y - start.y;

        if (x < 0) x *= -1;
        if (y < 0) y *= -1;

        return (x > y) ? x : y;
    }
    public static double DistanceDouble(Position start, Position end)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        return Math.Sqrt(x * x + y * y);
    }

    // == 연산자 오버로딩
    public static bool operator ==(Position position1, Position position2)
    {
        return position1.x == position2.x && position1.y == position2.y;
    }

    // != 연산자 오버로딩
    public static bool operator !=(Position position1, Position position2)
    {
        return !(position1 == position2);
    }

    //- 연산자 오버로딩
    public static Position operator -(Position startPosition, Position endPosition)
    {
        return new Position(startPosition.x - endPosition.x, startPosition.y - endPosition.y);
    }

    //+ 연산자 오버로딩
    public static Position operator +(Position startPosition, Position endPosition)
    {
        return new Position(startPosition.x + endPosition.x, startPosition.y + endPosition.y);
    }

    //간이 정규화. 3, 5일 경우 1, 1을 반환. -4, 7일 경우 -1, 1을 반환.
    public static Position Normalized(Position position)
    {
        int x = position.x;
        int y = position.y;

        if (x > 1) x = 1;
        else if (x < -1) x = -1;

        if (y > 1) y = 1;
        else if (y < -1) y = -1;

        return new Position(x, y);
    }
    public static Position Clamped(Position position)
    {
        int x = position.x;
        int y = position.y;

        if (x > 15) x = 15;
        else if (x < 0) x = 0;

        if (y > 15) y = 15;
        else if (y < 0) y = 0;

        return new Position(x, y);
    }
    public static Position Clamped(int x, int y)
    {        
        if (x > 15) x = 15;
        else if (x < 0) x = 0;

        if (y > 15) y = 15;
        else if (y < 0) y = 0;

        return new Position(x, y);
    }
    public static bool IsOutPosition(Position position)
    {
        if (position.x > 15 || position.x < 0 || position.y > 15 || position.y < 0)
            return true;
        return false;
    }
}
