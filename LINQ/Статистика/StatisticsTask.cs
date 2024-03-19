using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        private static double timeOnSlide = 0;
        private static bool sameSlide = false;

        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            return visits
                 .GroupBy(visit => visit.UserId)
                 .Select(group => group.OrderBy(x => x.DateTime)
                 .Bigrams())
                 .Select(visits =>
                 {
                     return visits
                    .Where(x => CheckSlideId(x) && x.Item1.SlideType == slideType)
                    .Select(visit => visit.Item2.DateTime.Subtract(visit.Item1.DateTime).TotalMinutes + timeOnSlide);
                 })
                 .SelectMany(x => x)
                 .Where(time => time >= 1 && time <= 120)
                 .ToList()
                 .DefaultIfEmpty(0)
                 .Median();
        }

        private static bool CheckSlideId((VisitRecord, VisitRecord) visit)
        {
            if (visit.Item1.SlideId.Equals(visit.Item2.SlideId))
            {
                sameSlide = true;
                timeOnSlide += visit.Item2.DateTime.Subtract(visit.Item1.DateTime).TotalMinutes;
                return false;
            }
            else if (sameSlide)
            {
                sameSlide = false;
                return true;
            }
            timeOnSlide = 0;
            return true;
        }
    }
}