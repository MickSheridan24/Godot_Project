
public interface IResourceBank
{
    int food { get; set; }
    int wealth { get; set; }
    int knowledge { get; set; }
    int insight { get; set; }

    int supply { get; set; }
    int pop { get; set; }

    string foodDisplay { get; set; }
    string popDisplay { get; set; }
}
