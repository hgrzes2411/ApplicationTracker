namespace ApplicationTracker.Models.Dashboard;

public class RecruitmentFunnelModel
{
    public int Applications { get; set; }

    public int Responses { get; set; }

    public int Interviews { get; set; }

    public int Offers { get; set; }


    public double ResponseRate =>
        Applications == 0
            ? 0
            : (double)Responses / Applications * 100;


    public double InterviewRate =>
        Responses == 0
            ? 0
            : (double)Interviews / Responses * 100;


    public double OfferRate =>
        Interviews == 0
            ? 0
            : (double)Offers / Interviews * 100;
}