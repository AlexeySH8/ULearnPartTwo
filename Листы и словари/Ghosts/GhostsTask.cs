using System;
using System.Text;

namespace hashes;

public class GhostsTask :
    IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
    IMagic
{
    private static byte[] array = new byte[] { 34, 11, 78 };
    private Document document = new Document("Cool Task", Encoding.UTF8, array);
    private Vector vector = new Vector(1.2, 1.3);
    private Segment segment = new Segment(new Vector(5, 6), new Vector(7, 8));
    private Cat cat = new Cat("Felix", "Persian", new DateTime(1, 1, 1));
    private Robot robot = new Robot("000");

    public void DoMagic()
    {
        array[0]++;
        vector.Add(new Vector(1.6, 1.8));
        segment.Start.Add(vector);
        cat.Rename("Persik");
        Robot.BatteryCapacity++;
    }

    Document IFactory<Document>.Create()
    {
        return document;
    }

    Vector IFactory<Vector>.Create()
    {
        return vector;
    }

    Segment IFactory<Segment>.Create()
    {
        return segment;
    }

    Cat IFactory<Cat>.Create()
    {
        return cat;
    }

    Robot IFactory<Robot>.Create()
    {
        return robot;
    }
}