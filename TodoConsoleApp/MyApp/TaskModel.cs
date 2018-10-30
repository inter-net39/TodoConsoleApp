using System;

namespace MyApp
{
    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AllDayAllDayActivity { get; set; }
        public bool? ActivityRank { get; set; }

        public TaskModel(string description, string startDate, string endDate, string activityRank)
        {

            //TODO: DODAĆ TUTAJ VALIDACJE???

            //Description = bez zmian z argumentem
            Description = description;
            
            //StrartDate = Jest po validacji.
            if (Validate.MatchDate(startDate))
            {
                StartDate = DateTime.Parse(startDate);
            }
            else
            {
                StartDate = null;
            }
         

            //EndDate = Jeżel EndDate niezdefiniowany to Całodniowe
            //TODO:VAL
            if (string.IsNullOrEmpty(endDate) || !Validate.MatchDate(endDate))
            {
                AllDayAllDayActivity = true;
                EndDate = null;
            }
            else
            {
                EndDate = DateTime.Parse(endDate);
            }

            //ActivityRank
            if (Validate.MatchBool(activityRank))
            {
                ActivityRank = bool.Parse(activityRank);
            }
            else
            {
                ActivityRank = null;
            }
        }

        public string Export()
        {
            return $"{Description},{Validate.DoValue(StartDate)},{Validate.DoValue(EndDate)},{Validate.DoValue(AllDayAllDayActivity)},{Validate.DoValue(ActivityRank)}";
        }
    }
}